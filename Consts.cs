using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullWebappAutomation
{
    class Consts
    {
        public const string webappSandboxLoginPageUrl = "https://app.sandbox.pepperi.com/#/";
        public const string webappSandboxHomePageUrl = "https://app.sandbox.pepperi.com/#/HomePage";

        public const int maxRetryCount = 15;
        public const int actionPerformanceLimit = 3;

        public enum ObjectType
        {
            transactions,
            items,
            transaction_lines,
            account_sellout,
            users,
            contacts,
            accounts,
            item_dimensions1,
            price_lists,
            item_dimensions2,
            item_prices,
            catalogs,
            account_catalogs,
            NULL,
            types,
            activities,
            type_safe_attribute,
            roles,
            account_users,
            images,
            attachments,
            profiles,
            inventory,
            account_inventory,
            user_defined_tables,
            special_price_lists
        }
    }
}
