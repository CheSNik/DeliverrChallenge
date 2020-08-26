using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverrChallenge
{
    class Program
    {     
        static void Main(string[] args)
        {
            Dictionary<string, int> orderList = new Dictionary<string, int>
            {
                { "Apples", 10 },
                { "Pears", 10 }
            };

            Orders orders = new Orders(orderList);
            WareHouse wareHouse1 = new WareHouse("owd");
            wareHouse1.SetInventoryAmount("Apples", 5);
            wareHouse1.SetInventoryAmount("Pears", 5);

            WareHouse wareHouse2 = new WareHouse("dw");
            wareHouse2.SetInventoryAmount("Apples", 5);
            wareHouse2.SetInventoryAmount("Pears", 5);

            Locations locations = new Locations();
            locations.setWareHouse(wareHouse1);
            locations.setWareHouse(wareHouse2);

            InventoryAllocator allocator = new InventoryAllocator();
            allocator.AllocateOrderAmongLocations(orders, locations);

            Console.ReadKey();
        }
    }
}
