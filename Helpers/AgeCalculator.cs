namespace Ava.Helpers
{
    public static class AgeCalculator
    {
        /// <summary>
        /// Calculates the age in years based on the provided birthdate.
        /// </summary>
        /// <param name="birthdate">The birthdate to calculate age from.</param>
        /// <returns>The age in years.</returns>
        public static int CalculateAge(DateTime birthdate)
        {
            if (birthdate > DateTime.Now)
            {
                throw new ArgumentException("Birthdate cannot be in the future.", nameof(birthdate));
            }

            int age = DateTime.Now.Year - birthdate.Year;

            // Check if the birthday has occurred this year
            if (DateTime.Now.DayOfYear < birthdate.DayOfYear)
            {
                age--;
            }

            return age;
        }
    }
}
