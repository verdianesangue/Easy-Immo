using BU.Services;
using DAL.DB;

internal class Program
{
    static void Main(string[] args)
    {
        var Biens = BU.Services.BienService.GetBiens();
        foreach (var bien in Biens) {

            Console.WriteLine(bien.IdBien);
            Console.WriteLine(bien.PrixBien);
            Console.WriteLine(bien.DescriptionBien);

        }
            


    }
}