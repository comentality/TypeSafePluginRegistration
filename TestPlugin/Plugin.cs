using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Messages;

[assembly: Microsoft.Xrm.Sdk.Client.ProxyTypesAssemblyAttribute()]

namespace TestPlugin
{
    using RegistrationTarget;

    /// <summary>
    /// Рассчитывает сумму скидки и общую сумму при обновлении продукта КП.
    /// 
    /// Register:
    ///     Create of QuoteDetail
    /// 
    ///     Update of QuoteDetail:
    ///         new_discount, priceperunit, quantity, tax
    ///         
    ///         Pre Image:
    ///             new_discount, priceperunit, quantity, tax, ManualDiscountAmount
    /// </summary>

    public partial class Plugin : IPlugin, IRegistrationTarget
    {
        internal IOrganizationService Service;
        internal crmContext XrmContext;

        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            ((IProxyTypesAssemblyProvider)context).ProxyTypesAssembly = typeof(Contact).Assembly;

            ITracingService t = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            if (!(context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity))
            {
                return;
            }

            Entity target = (Entity)context.InputParameters["Target"];

            var targetQD = target.ToEntity<QuoteDetail>();
            var preImageQD = targetQD;

            if (context.MessageName == MessageName.Update)
            {
                Entity preImage = (Entity)context.PreEntityImages["Image"];
                preImageQD = preImage.ToEntity<QuoteDetail>();
            }

            if (context.PrimaryEntityName != QuoteDetail.EntityLogicalName)
            {
                throw new InvalidPluginExecutionException("Plugin registered for wrong entity!");
            }
            if (context.MessageName != MessageName.Create &&
                context.MessageName != MessageName.Update)
            {
                throw new InvalidPluginExecutionException("Plugin registered for wrong message!");
            }

            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            Service = serviceFactory.CreateOrganizationService(context.UserId);
            XrmContext = new crmContext(Service);

            // Plugin starts here
            var tax = targetQD.Tax ?? preImageQD.Tax ?? new Money(0);
            var pricePerUnit = targetQD.PricePerUnit ?? preImageQD.PricePerUnit ?? new Money(0);
            var quantity = targetQD.Quantity ?? preImageQD.Quantity ?? 0;
            var discountPercent = targetQD.new_discount ?? preImageQD.new_discount ?? 0;
            var discountAmount = targetQD.ManualDiscountAmount ?? preImageQD.ManualDiscountAmount ?? new Money(0);

            // возможно мы создаем из файла Эксель. Нужно попробовать посчитать процент скидки из суммы скидки.
            var isCreateAndNoDiscount = discountPercent == 0 && context.MessageName == MessageName.Create;

            var discountAmountDeci = 0.0m;

            if (isCreateAndNoDiscount)
            {
                discountAmountDeci = discountAmount.Value;

                if (discountAmountDeci != 0 && (pricePerUnit.Value * quantity + tax.Value) != 0)
                {
                    discountPercent = (double)(100 / (pricePerUnit.Value * quantity + tax.Value));

                    //setting up
                    targetQD.new_discount = discountPercent;
                }
            }
            else
            {
                discountAmountDeci = (pricePerUnit.Value * quantity + tax.Value) * (decimal)discountPercent / 100;

                //setting up
                targetQD.new_discount_amount = new Money(discountAmountDeci);
                targetQD.ManualDiscountAmount = new Money(discountAmountDeci);
            }


            var totalAmountDeci = (pricePerUnit.Value * quantity + tax.Value) - discountAmountDeci;

            targetQD.new_total_amount = new Money(totalAmountDeci);
        }
    }
}
