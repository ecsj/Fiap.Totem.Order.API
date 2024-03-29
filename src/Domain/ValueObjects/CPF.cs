namespace Domain.ValueObjects;

public class CPF
{
    private readonly string value;

    public CPF(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("CPF não pode ser vazio ou nulo");

        value = value.Replace(".", "").Replace("-", "");

        if (!IsValidCPF(value))
            throw new ArgumentException("CPF inválido");

        this.value = value;
    }

    public override string ToString()
    {
        return FormatCPF(value);
    }

    private static bool IsValidCPF(string cpf)
    {
        if (cpf.Length != 11)
            return false;

        bool hasSameDigit = true;
        for (int i = 1; i < cpf.Length; i++)
        {
            if (cpf[i] != cpf[0])
            {
                hasSameDigit = false;
                break;
            }
        }

        if (hasSameDigit)
            return false;

        int[] multipliers1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multipliers2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCPF = cpf.Substring(0, 9);
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += int.Parse(tempCPF[i].ToString()) * multipliers1[i];
        }

        int remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        string digit1 = remainder.ToString();
        tempCPF += digit1;
        sum = 0;

        for (int i = 0; i < 10; i++)
        {
            sum += int.Parse(tempCPF[i].ToString()) * multipliers2[i];
        }

        remainder = sum % 11;
        remainder = remainder < 2 ? 0 : 11 - remainder;

        string digit2 = remainder.ToString();
        return cpf.EndsWith(digit1 + digit2);
    }

    private static string FormatCPF(string cpf)
    {
        return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
    }
}
