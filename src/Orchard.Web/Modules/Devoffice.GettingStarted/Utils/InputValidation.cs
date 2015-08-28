using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Devoffice.GettingStarted.Utils
{
    public class InputValidation
    {
        public static readonly string[] validPlatforms = {
                                                             "option-android",
                                                             "option-dotnet",
                                                             "option-ios",
                                                             "option-node" ,  
                                                             "option-php",
                                                             "option-python",
                                                             "option-ruby"   
                                                 };

        public static readonly string[] validProducts = {
                                                             "excel",
                                                             "outlook",
                                                             "powerpoint",
                                                             "word" 
                                                 };

        public static bool isValidPlatform(string platformId)
        {
            if (!string.IsNullOrEmpty(platformId))
            {
                return validPlatforms.Contains(platformId);
            }
            return false;
        }
        public static bool isValidProduct(string product)
        {
            if (!string.IsNullOrEmpty(product))
            {
                return validProducts.Contains(product);
            }
            return false;
        }
    }
}