using Microsoft.AspNetCore.Identity;

public class UserService {
    private readonly PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();

    public bool VerifyPassword(string hashedPassword, string providedPassword) {
        var verificationResult = _passwordHasher.VerifyHashedPassword("", hashedPassword, providedPassword);
        return verificationResult == PasswordVerificationResult.Success;
    }
}