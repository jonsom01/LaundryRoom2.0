# LaundryRoom2.0

It's an application that handles bookings for a laundry room. It's designed to have an "analog" feeling. When you click on an empty space,and press your code, your "bolt" turns up in that space, and that means you've booked that time.

Set up:

1. Download the whole project and open it using VS 2017.
2. Create your own database using EF code first (or set one up manually (check the models for requirements)).
3. Add your db connection string to your appsettings.json
4. Add some users to your db. I have used the following code to make random passwords with hash and salt:

        private string CreatePass()
        {
            Random RandomPIN = new Random();
            var RandomPINResult = RandomPIN.Next(0, 9999).ToString();
            return RandomPINResult.PadLeft(4, '0');

        }

        private string CreateSalt(int size)
        {
            var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);

        }

        private string CreateHash(string pass, string salt)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(pass + salt);
            var sha256HashString = System.Security.Cryptography.SHA256.Create();
            byte[] hash = sha256HashString.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
