//====================================================
//| Downloaded From                                  |
//| Visual C# Kicks - http://www.vcskicks.com/       |
//| License - http://www.vcskicks.com/license.html   |
//====================================================
using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsApplication1
{
    public static class CreditCardVerification
    {
        /// <summary>
        /// To make sure the cardNumber comes in the correct format
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public static bool IsValidCardNumber(int cardNumber)
        {
            return IsValidCardNumber(cardNumber.ToString());
        }

        /// <summary>
        /// Used for the Shop to verify if a Creditcardnumber is valid or not
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns>bool IsValidCardNumber</returns>
        public static bool IsValidCardNumber(string cardNumber)
        {
            cardNumber = cardNumber.Replace(" ", "");

            //FIRST STEP: Double each digit starting from the right
            int[] doubledDigits = new int[cardNumber.Length / 2];
            int k = 0;
            for (int i = cardNumber.Length - 2; i >= 0; i -= 2)
            {
                int digit = int.Parse(cardNumber[i].ToString());
                doubledDigits[k] = digit * 2;
                k++;
            }

            //SECOND STEP: Add up separate digits
            int total = 0;
            foreach (int i in doubledDigits)
            {
                string number = i.ToString();
                for (int j = 0; j < number.Length; j++)
                {
                    total += int.Parse(number[j].ToString());
                }
            }

            //THIRD STEP: Add up other digits
            int total2 = 0;
            for (int i = cardNumber.Length - 1; i >= 0; i -= 2)
            {
                int digit = int.Parse(cardNumber[i].ToString());
                total2 += digit;
            }

            //FOURTH STEP: Total
            int final = total + total2;

            return final % 10 == 0; //Well formed will divide evenly by 10
        }
    }
}
