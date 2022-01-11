using System;

/*
# Option Calculator
-  Price a one-factor European Option
-  Will consist of 4 classes:
    1. Mediator: coordinate data between the classes
    2. Factory: initialises the data
    3. Computation: calculate price using BS formula
    4. Display: display the results
 */


//  Gaussian Prob. and cumulative distribution functions
public class Gauss
{
    static public double n(double x)
    {
        double A = 1.0 / Math.Sqrt(2.0 * 3.1415);
        return A * Math.Exp(-x * x * 0.5);
    }
    static public double N(double x)
    {
        // approximation of cumulative normal dist
        double a1 = 0.4361836;
        double a2 = -0.1201676;
        double a3 = 0.9372980;
        double k = 1.0 / (1.0 + (0.33267 * x));

        if (x >= 0.0)
        {
            return 1.0 - n(x) * (a1 * k + (a2 * k * k) + (a3 * k * k * k));
        }
        else
        {
            return 1.0 - N(-x);
        }

    }
}

public class Option
{
    private double r; // interest rate
    private double s; // volatility
    private double K; // strike price
    private double T; // expiry date
    private double b; // cost of carry
    private string type; // call or put

    // class method
    private double CallPrice(double U)
    {
        double denom = s * Math.Sqrt(T);
        double d1 = (Math.Log(U / K) + (b + (s * s) * 0.5) * T) / denom;
        double d2 = d1 - denom;
        // return the call price
        return (U * Math.Exp((b - r) * T) * Gauss.N(d1)) - (K * Math.Exp(-r * T) * Gauss.N(d2));
    }

    private double PutPrice(double U)
    {
        double denom = s * Math.Sqrt(T);
        double d1 = (Math.Log(U / K) + (b + (s * s) * 0.5) * T) / denom;
        double d2 = d1 - denom;
        // return the put price
        return (K * Math.Exp((-r * T) * Gauss.N(-d2)) - (U * Math.Exp(b - r) * Gauss.N(-d1)));

    }
    public void init()
    {
        // initialise default values
        r = 0.08;
        s = 0.30;
        K = 65.0;
        T = 0.25;
        b = r;
        type = "C";
    }
    // instances
    public Option()
    {
        // a default call option
        init();
    }
    public Option(string optionType)
    {
        init();
        type = optionType;
        if (type == "c")
        {
            type = "C";
        }
    }
    public Option(string optionType, double expiry, double strike,
                    double costOfCarry, double interest, double volatility)
    {
        // option instance
        type = optionType;
        T = expiry;
        K = strike;
        b = costOfCarry;
        r = interest;
        s = volatility;
    }
    public Option(string optionType, string underlying)
    {
        init();
        type = optionType;
    }
    // method to calculate option price and sensitivities
    public double Price(double U)
    {
        if (type == "1")
        {
            return CallPrice(U);
        }
        else
        {
            return PutPrice(U);
        }
    }
}

public interface IOptionFactory
{
    Option create();
}

public class ConsoleEuropeanOptionFactory : IOptionFactory
{
    public Option create()
    {
        Console.WriteLine("Data for option object");
        double r; // interest rate
        double s; // volatility
        double K; // strike price
        double T; // expiry date
        double b; // cost of carry
        string type; // call or put

        // read input values
        Console.Write("Strike: ");
        K = Convert.ToDouble(Console.ReadLine());
        Console.Write("Volatility: ");
        s = Convert.ToDouble(Console.ReadLine());
        Console.Write("Interest Rate: ");
        r = Convert.ToDouble(Console.ReadLine());
        Console.Write("Cost of Carry: ");
        b = Convert.ToDouble(Console.ReadLine());
        Console.Write("Expity Date: ");
        T = Convert.ToDouble(Console.ReadLine());
        Console.Write("Call or Put: ");
        type = Convert.ToString(Console.ReadLine());

        Option opt = new Option(type, T, K, b, r, s);

        return opt;
    }
}

// mediator entity to coordinate the data and flow
public struct Mediator
{
    static IOptionFactory getFactory()
    {
        return new ConsoleEuropeanOptionFactory();
    }
    public void calculate()
    {
        // 1. choose how the data in the option will be created
        IOptionFactory fac = getFactory();
        // 2. create the option
        Option myOption = fac.create();
        // 3. get underlying price
        Console.Write("Enter the underlying price: ");
        double S = Convert.ToDouble(Console.ReadLine());
        // 4. display result
        Console.WriteLine("Price: {0}", myOption.Price(S));

    }
}

// main method (application entry point)
class RunOption
{
    static void Main()
    {
        // client delegates to the mediator
        Mediator med = new Mediator();
        med.calculate();
    }
}