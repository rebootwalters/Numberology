using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer;

namespace DataLayerTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            using (BusinessLogicLayer.BLLContext ctx = new BusinessLogicLayer.BLLContext())
            {
               
                
              
                while (true)
                {
                    Console.Write("Enter a number ID:");
                    var indata = Console.ReadLine();
                    int id;
                    bool rv = int.TryParse(indata, out id);
                    if (rv)
                    {
                        var r = ctx.Numbers.FindNumber(id);
                        if (r == null)
                        {
                            Console.WriteLine("No Record with that ID was found - Try Again");
                        }
                        else
                        {
                            Console.WriteLine($"{r.ID}  {r.Name}  {r.Doublestuff} {r.Floatstuff}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The Number you Entered was not a valid integer - try again");
                    }
                }





            }
            

            

        }
    }
}
