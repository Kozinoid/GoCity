/**/

using System;

namespace GoCity
{
    class Program
    {
        static void Main(string[] args)
        {
            DataModel data = new DataModel();               // Data
            StringReader reader = new StringReader(file);   // File read simulator
            data.LoadData(reader);                          // Read data 'from file'

            // Test Output loaded data
            data.TestOutput();

            // Read Path-to-find count
            int pathToFindCount = reader.ReadInt();

            // Path find cycle
            for(int i = 0; i < pathToFindCount; i++)
            {
                // Read citie names for each test
                string fromCity = reader.ReadString();  // From
                string toCity = reader.ReadString();    // To
                Console.WriteLine("From: " + fromCity + " to: " + toCity);

                // Get indexes for 'from' and 'to' cities
                int fromIndex = data.GetCityIndex(fromCity);
                int toIndex = data.GetCityIndex(toCity);

                // Call algorithm
                GetPath(fromIndex, toIndex, data);
            }
        }

        //**********************************************************************************************************************
        // ****************************************  Dijkstra's algorithm  *****************************************************
        static void GetPath(int fromIndex, int toIndex, DataModel data) 
        {
            // Init result array. 
            data.InitResultArray(fromIndex);
            
            do
            {
                int currentIndex = -1;
                int min = 2000000;
                // Find 'unvisited' city node with minimum weight
                for (int i = 0; i < data.cityCount; i++)
                {
                    if (!data.marked[i])
                    {
                        if (data.result[i] < min)
                        {
                            min = data.result[i];
                            currentIndex = i;
                        }
                    }
                }
                // if not all city nodes was 'visited', we get new one for processing
                if (currentIndex >= 0) WorkWithCity(data, currentIndex);
                else break; // All nodes was 'visited'
            }
            while (true);

            Console.WriteLine(string.Format("Min weight = {0}", data.result[toIndex]));
        }

        // Work with current city node
        static void WorkWithCity(DataModel data, int currentIndex)
        {
            for (int i = 0; i < data.cityCount; i++)
            {
                if (i != currentIndex) // if it is not current node
                {
                    if (!data.marked[i])    // if node was not wisited
                    {
                        if ((data.weight[currentIndex, i] + data.result[currentIndex]) < data.result[i])    // If new weight < old weight
                        {
                            data.result[i] = data.weight[currentIndex, i] + data.result[currentIndex];      // Set new Weight
                        }
                    }
                }
            }
            // Mark current node as 'visited'
            data.marked[currentIndex] = true;
        }
        //***********************************************************************************************************************

        // DATA MODEL. This class loads and contains all data.
        class DataModel
        {
            // Vars
            public int testCount;   // Test count (What for???)
            public int cityCount;   // City count

            // Main data
            public City[] cities;   // City names and IDs
            public int[,] weight;   // transportation cost (weight) between pairs of cities
            public bool[] marked;   // Marks for Dijkstra's algorithm
            public int[] result;    // Resul array of weights

            // Load all data. Init all arrays
            public void LoadData(StringReader reader)
            {
                //------------------ Get data ---------------
                testCount = reader.ReadInt();
                Console.WriteLine(string.Format("Number of tests: {0}", testCount));
                cityCount = reader.ReadInt();
                Console.WriteLine(string.Format("Number of cities: {0}", cityCount));

                // Init arrays
                cities = new City[cityCount];
                weight = new int[cityCount, cityCount];
                marked = new bool[cityCount];
                result = new int[cityCount];
                for (int i = 0; i < cityCount; i++)
                {
                    for (int j = 0; j < cityCount; j++)
                    {
                        weight[i, j] = 2000000;
                    }
                }

                // Get City data
                for (int i = 0; i < cityCount; i++)
                {
                    cities[i].id = i + 1;
                    cities[i].cityName = reader.ReadString();
                    int neighbors = reader.ReadInt();
                    for (int j = 0; j < neighbors; j++)
                    {
                        int idTo = reader.ReadInt();
                        int edgeWieght = reader.ReadInt();
                        weight[i, idTo - 1] = edgeWieght;
                    }
                }
            }

            // Get city index
            public int GetCityIndex(string cityName)
            {
                int index = -1;
                for (int i = 0; i < cityCount; i++)
                {
                    if (cities[i].cityName == cityName)
                    {
                        index = i;
                        break;                    }
                }

                return index;
            }

            // Init result weight array
            public void InitResultArray(int cityFrom)
            {
                // All city nodes set to 'unvisited' state and set costs (weights) to 'very big'. 
                for (int i = 0; i < cityCount; i++)
                {
                    marked[i] = false;
                    result[i] = 2000000;
                }
                result[cityFrom] = 0;   // initial city node weght set to '0'
            }

            // Test for Console output weight matrix
            public void TestOutput()
            {
                Console.Write("|          ");
                for (int i = 0; i < cityCount; i++)
                {
                    Console.Write(string.Format("|{0,10}", cities[i].cityName));
                }
                Console.WriteLine("|");
                for (int i = 0; i < cityCount; i++)
                {
                    Console.Write(string.Format("|{0,10}|", cities[i].cityName));
                    for (int j = 0; j < cityCount; j++)
                    {
                        Console.Write(string.Format("{0, 10}|", weight[i, j]));
                    }
                    Console.WriteLine();
                }
            }
        }

        // --------------  City data structure  ----------------
        struct City
        {
            public int id;
            public string cityName;
        }

        //**************************************************************************************************************
        //*****************************************  FILE DATA FOR TEST  ***********************************************
        // File data
        const string file = "1 4 gdansk 2 2 1 3 3 bydgoszcz 3 1 1 3 1 4 4 torun 3 1 3 2 1 4 1 warszawa 2 2 4 3 1 2 gdansk warszawa bydgoszcz warszawa";

        // File reading emulation
        class StringReader
        {
            int pointer;
            string[] fileData;

            public StringReader(string file)
            {
                fileData = file.Split(' ');
                pointer = 0;
            }

            public int ReadInt()
            {
                string data = fileData[pointer];
                pointer++;
                return int.Parse(data);
            }

            public string ReadString()
            {
                string data = fileData[pointer];
                pointer++;
                return data;
            }
        }
    }
}
            