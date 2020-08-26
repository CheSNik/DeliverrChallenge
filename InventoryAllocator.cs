using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;

namespace DeliverrChallenge
{
    public class InventoryAllocator
    {

        private Orders _orders;
        private Locations _locations;
        public int OrderAmount { get; private set; }
        /// <summary>
        /// Object receives orders and locations, than distributes order among locations
        /// </summary>
        /// <param name="orders"></param>
        /// <param name="locations"></param>
        public bool AllocateOrderAmongLocations(Orders orders, Locations locations)
        {
            _orders = orders;
            _locations = locations;

            //validate if all orders has values >=0
            if (!ValidateOrders(_orders))
            {
                Console.WriteLine("Order has negative amount");
                return false;
            }
                

            //loop to iterate through every item in the order
            foreach (var orderItem in _orders._orderList)
            {
                OrderAmount = orderItem.Value;
                int i = 0;

                //loop iterates as need amount of warehouses in a presorted list to store order 
                while (OrderAmount != 0)
                {

                    //call method allocateItem for each next warehouse
                    OrderAmount = locations.locationsList.ElementAt(i).AllocateItem(orderItem.Key, OrderAmount);

                    if (OrderAmount == -1) //AllocateItem() returns (-1) if there are no inventoryName in warehouse
                    {
                        Console.WriteLine("No inventory set for" + orderItem.Key);
                        break;
                    }

                    if (i == locations.locationsList.Count - 1 && OrderAmount > 0)
                    {
                        Console.WriteLine("No inventory space for" + OrderAmount + " of" + orderItem.Key);
                        break;
                    }
                    //go to next warehouse
                    i++;
                }
            }

            foreach(var warehouse in locations.locationsList)
            {
                warehouse.PrintStoredItems();
            }

            return true;
        }

        private bool ValidateOrders(Orders orders)
        {
            bool isValid = true;

            foreach (var order in orders._orderList)
            {
                if (order.Value < 0)
                    isValid = false;
            }
            return isValid;
        }

    }

    /// <summary>
    /// Order object stores collection of orders, where order is a KeyValuePare of "Name" & "Amount"
    /// </summary>
    public class Orders
    {
        //Stores order details: item and quantity of items
        public Dictionary<string, int> _orderList;


        public Orders(Dictionary<string, int> orderList)
        {
            _orderList = orderList;
        }

    }

    

    /// <summary>
    /// Warehouse object stores Inventory in "_storedInventory" and control available spaces for inventory in "InventoryAmounts"
    /// </summary>
    public class WareHouse
    {
        public string Name { get; }
        private Dictionary<string, int> InventoryAmounts { get; } = new Dictionary<string, int>();
        private Dictionary<string, int> StoredInventory { get; } = new Dictionary<string, int>();
        public WareHouse(string wareHouseName)
        {
            Name = wareHouseName;
        }
        /// <summary>
        /// Retrive amount of specified by name item
        /// </summary>
        /// <param name="inventoryName">Name of item (inventory)</param>
        /// <returns></returns>
        public int GetInventoryAmounts(string inventoryName)
        {
            return InventoryAmounts[inventoryName];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inventoryName"></param>
        /// <returns></returns>
        public int GetStoredItems(string inventoryName)
        {
            return StoredInventory[inventoryName];
        }

        /// <summary>
        /// Set "free space" for the item in Inventoryamount Dictionary
        /// </summary>
        /// <param name="inventoryName">Input item name</param>
        /// <param name="inventoryAmount">Input item amount</param>
        public void SetInventoryAmount(string inventoryName, int inventoryAmount)
        {
            if (InventoryAmounts.ContainsKey(inventoryName))
                //if we add more inventory to a warehouse to an existing item 
                InventoryAmounts[inventoryName] += inventoryAmount;
            else
                //if we add brand new item
                InventoryAmounts.Add(inventoryName, inventoryAmount);
        }

        /// <summary>
        /// Allocate items from the order to a
        /// </summary>
        /// <param name="itemName">Input item name</param>
        /// <param name="itemAmount">Input item amount</param>
        /// <returns></returns>
        public int AllocateItem(string inventoryName, int inventoryAmount)
        {
            if (InventoryAmounts.ContainsKey(inventoryName))
            {
                int notAllocated = inventoryAmount;
                //Check if there is free space in inventory for certain item
                if (InventoryAmounts[inventoryName] >= notAllocated)
                {
                    StoredInventory.Add(inventoryName, notAllocated);
                    InventoryAmounts[inventoryName] -= notAllocated;
                    return 0;
                }
                else // inventoryAmounts[inventoryName] < inventoryAmount
                {
                    notAllocated = notAllocated - InventoryAmounts[inventoryName]; //decrease not allocated item variable
                    StoredInventory.Add(inventoryName, InventoryAmounts[inventoryName]); //add allocated item to a stored items Collection
                    InventoryAmounts[inventoryName] = 0; //assign to zero available space for storage.
                    return notAllocated;
                }
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Prints output
        /// </summary>
        public void PrintStoredItems()
        {
            Console.WriteLine("Ware house name is " + Name);
            foreach (var storedItem in StoredInventory)
            {
                Console.WriteLine("{" + storedItem.Key + ", " + storedItem.Value + "}");
            }
        }
    }

    public class Locations
        {
        /// <summary>
        /// List stores possible locations
        /// </summary>
        public List<WareHouse> locationsList = new List<WareHouse>();

        /// <summary>
        /// Method add Object of WareHouse type to a List that stores possible locations
        /// It is assumed that List is sorted by expenses to store items
        /// </summary>
        /// <param name="any">Object type WareHouse</param>
        public void setWareHouse (WareHouse any)
        {
            locationsList.Add(any);
        }
    }
}
