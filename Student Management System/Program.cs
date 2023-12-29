using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Security.Cryptography;

namespace StudentManagementSystem
{
    class Program
    {
        class Person
        {
            public string name { get; protected set; }
            public int age { get; protected set; }
            public string bdate { get; protected set; }
            public string gender { get; protected set; }
            public string section { get; protected set; }
            public string studentId { get; protected set; }

            public Person(string name, int age, string bdate, string gender, string section, string studentId)
            {
                this.name = name;
                this.age = age;
                this.bdate = bdate;
                this.gender = gender;
                this.section = section;
                this.studentId = studentId;
            }

            public string returnDetails()
            {
                return $"{name} | {age} | {bdate} | {gender} | BSIT {section} | {studentId}";
            }
            public void setName(string name)
            {
                this.name = name;
            }
            public void setAge(int age)
            {
                this.age = age;
            }
            public void setBdate(string bdate)
            {
                this.bdate = bdate;
            }
            public void setGender(string gender)
            {
                this.gender = gender;
            }
            public void setSection(string section)
            {
                this.section = section;
            }
            public void setStudentId(string studentId)
            {
                this.studentId = studentId;
            }
        }
        class Manager
        {
            public List<Person> people;

            public Manager()
            {
                people = new List<Person>();

                // initial database 
                people.Add(new Person("Emierose Silvestre", 18, "F", "09/09/2005", "1A", "0323-3551"));
                people.Add(new Person("Maria Dolores Shane Acabado", 19, "F", "12 / 08 / 2004", "1A", "0323-3580"));
                people.Add(new Person("Ainie Villanueva", 19, "F", "11/18/2004", "1A", "0323-3558"));
                people.Add(new Person("Justin Rain Bundalian", 18, "M", "05/20/2005", "1A", "0323-2652"));
                people.Add(new Person("Karl Angelo Jayag", 19, "M", "10 / 23 / 2004", "1A", "0323 - 3557"));
                people.Add(new Person("Mark Angelo Abangan", 19, "M", "08/30/2004", "1A", "0323-2379"));
                people.Add(new Person("Ella Mae Alad", 19, "F", "08/27/2004", "1A", "0323-3569"));
                people.Add(new Person("Janna Abbey Santos", 18, "F", "01/24/2005", "1A", "0323-3572"));


                printMenu();
            }
            public void printMenu()
            {
                Console.Clear();

                string[] menuOptions = new string[]
                {
                    "View All Students",
                    "Add Students",
                    "Edit Students",
                    "Search Students",
                    "Remove Students",
                    "Exit",
                };

                Console.WriteLine("╔══════════════════════════════════════╗");
                Console.WriteLine("║ STUDENT MANAGEMENT FOR IT STUDENTS   ║");
                Console.WriteLine("╠══════════════════════════════════════╣");
                Console.WriteLine("║ OPTIONS                              ║");

                for (int i = 0; i < menuOptions.Length; i++)
                {
                    Console.WriteLine($"║   {i + 1}. {menuOptions[i],-31} ║");
                }

                Console.WriteLine("╚══════════════════════════════════════╝");
                Console.Write("Choose Option: ");

                bool tryParse = int.TryParse(Console.ReadLine(), out int menuOption);

                if (tryParse)
                {
                    if (menuOption == 1)
                    {
                        PrintAll();
                    }
                    else if (menuOption == 2)
                    {
                        AddPerson();
                    }
                    else if (menuOption == 3)
                    {
                        EditPerson();
                    }
                    else if (menuOption == 4)
                    {
                        SearchPerson();
                    }
                    else if (menuOption == 5)
                    {
                        RemovePerson();
                    }

                    if (menuOption >= 1 && menuOption <= menuOptions.Length - 1)
                    {
                        printMenu();
                    }
                }
                else
                {
                    OutputMessage("Incorrect menu choice.");
                    printMenu();
                }
            }


            public void PrintAll()
            {
                StartOption("Printing all students");

                if (!isSystemEmpty())
                {
                    Console.WriteLine("Filter Options:");
                    Console.WriteLine("1. Filter by Gender");
                    Console.WriteLine("2. Filter by Age");
                    Console.WriteLine("3. Filter by Section");
                    Console.WriteLine("4. Show All");

                    Console.Write("Choose Filter Option: ");
                    bool tryParseFilterOption = int.TryParse(Console.ReadLine(), out int filterOption);

                    if (tryParseFilterOption)
                    {
                        switch (filterOption)
                        {
                            case 1:
                                PrintFilteredUsersByGender();
                                break;
                            case 2:
                                PrintFilteredUsersByAge();
                                break;
                            case 3:
                                PrintFilteredUsersBySection();
                                break;
                            case 4:
                                PrintUsers(people);
                                break;
                            default:
                                OutputMessage("Invalid filter option.");
                                break;
                        }
                    }
                    else
                    {
                        OutputMessage("Invalid filter option.");
                    }
                }

                FinishOption();
            }

            public void PrintFilteredUsersByAge()
            {
                int targetAge;
                bool isValidAge;

                do
                {
                    Console.Write("Enter Age: ");
                    string ageInput = Console.ReadLine();
                    isValidAge = int.TryParse(ageInput, out targetAge);

                    if (!isValidAge)
                    {
                        OutputMessage("Invalid age. Please enter a valid integer.");
                    }

                } while (!isValidAge);

                var filteredUsers = people.FindAll(person => person.age == targetAge);
                PrintUsers(filteredUsers);
            }

            public void PrintFilteredUsersByGender()
            {
                Console.Write("Enter Gender (M/F): ");
                string genderFilter = Console.ReadLine()?.ToUpper();

                //3rd column si gender, since its index 0 based yon
                var filteredUsers = FilterByColumn(2, genderFilter);
                PrintUsers(filteredUsers);
            }

            public void PrintFilteredUsersBySection()
            {
                Console.Write("Enter Section (ex: BSIT 1A): ");
                string sectionFilter = Console.ReadLine().ToUpper();

                var filteredUsers = FilterByColumn(4, sectionFilter);
                PrintUsers(filteredUsers);
            }

            private List<Person> FilterByColumn(int columnIndex, string filter)
            {
                return people.FindAll(person => GetColumnValue(person, columnIndex) == filter);
            }

            private string GetColumnValue(Person person, int columnIndex)
            {
                string[] columns = person.returnDetails().Split('|').Select(column => column.Trim()).ToArray();

                if (columnIndex >= 0 && columnIndex < columns.Length)
                {
                    return columns[columnIndex];
                }
                else
                {
                    return string.Empty;
                }
            }

            public void PrintUsers(List<Person> users)
            {
                if (users.Count > 0)
                {
                    Console.WriteLine("Students:");
                    for (int i = 0; i < users.Count; i++)
                    {
                        Console.WriteLine(i + 1 + ". " + users[i].returnDetails());
                    }
                }
                else
                {
                    Console.WriteLine("No students match the filter criteria.");
                }
            }

            public void AddPerson()
            {
                StartOption("Adding a students:");

                try
                {
                    Person person = returnPerson();

                    if (person != null)
                    {
                        people.Add(person);
                        Console.WriteLine("Successfully added a student.");
                        FinishOption();
                    }
                    else
                    {
                        OutputMessage("Something has went wrong.");
                        AddPerson();
                    }
                }
                catch (Exception)
                {
                    OutputMessage("Something has went wrong.");
                    AddPerson();
                }
            }
            public void EditPerson()
            {
                StartOption("Editing a student:");

                if (!isSystemEmpty())
                {
                    PrintAllUsers();

                    try
                    {
                        Console.Write("Enter a number: ");
                        int indexSelection = Convert.ToInt32(Console.ReadLine());

                        indexSelection--;


                        if (indexSelection >= 0 && indexSelection <= people.Count - 1)
                        {
                            try
                            {
                                Person person = returnPerson();

                                if (person != null)
                                {
                                    people[indexSelection] = person;
                                    Console.WriteLine("Successfully edited a student.");
                                    FinishOption();
                                }
                                else
                                {
                                    OutputMessage("Something has went wrong.");
                                    EditPerson();
                                }
                            }
                            catch (Exception)
                            {
                                OutputMessage("Something has went wrong.");
                                EditPerson();
                            }
                        }
                        else
                        {
                            OutputMessage("Enter a valid index range.");
                            EditPerson();
                        }
                    }
                    catch (Exception)
                    {
                        OutputMessage("Something went wrong.");
                        EditPerson();
                    }
                }
                else
                {
                    OutputMessage("");
                }
            }
            public void SearchPerson()
            {
                StartOption("Searching student");


                if (!isSystemEmpty())
                {
                    Console.Write("Enter a name: ");
                    string nameInput = Console.ReadLine();

                    bool bFound = false;

                    if (!string.IsNullOrEmpty(nameInput))
                    {
                        for (int i = 0; i < people.Count; i++)
                        {
                            if (people[i].name.ToLower().Contains(nameInput.ToLower()))
                            {
                                Console.WriteLine(people[i].returnDetails());
                                bFound = true;
                            }
                        }

                        if (!bFound)
                        {
                            Console.WriteLine("No students found with that name.");
                        }

                        FinishOption();
                    }
                    else
                    {
                        OutputMessage("Please enter a name.");
                        SearchPerson();
                    }
                }
                else
                {
                    OutputMessage("");
                }
            }
            public void RemovePerson()
            {
                StartOption("Removing a student:");

                if (!isSystemEmpty())
                {
                    PrintAllUsers();

                    Console.Write("Enter a number: ");
                    int index = Convert.ToInt32(Console.ReadLine());
                    index--;

                    if (index >= 0 && index <= people.Count - 1)
                    {
                        people.RemoveAt(index);
                        Console.WriteLine("Successfully removed a person.");

                        FinishOption();
                    }
                    else
                    {
                        OutputMessage("Enter a valid number inside the range.");
                        RemovePerson();
                    }
                }
                else
                {
                    OutputMessage("");
                }
            }
            public void FinishOption()
            {
                Console.WriteLine(Environment.NewLine + "You have finished this option. Press <Enter> to return to the menu.");
                Console.ReadLine();
                Console.Clear();
            }
            public void StartOption(string message)
            {
                Console.Clear();
                Console.WriteLine(message + Environment.NewLine);
            }
            public void OutputMessage(string message)
            {
                if (message.Equals(string.Empty))
                {
                    Console.Write("Press <Enter> to return to the menu.");
                }
                else
                {
                    Console.WriteLine(message + " Press <Enter> to try again.");
                }
                Console.ReadLine();
                Console.Clear();
            }
            public void PrintAllUsers()
            {
                for (int i = 0; i < people.Count; i++)
                {
                    Console.WriteLine(i + 1 + ". " + people[i].returnDetails());
                }
            }
            public Person returnPerson()
            {
                Console.Write("Complete Name: ");
                string nameInput = Console.ReadLine();

                Console.Write("Age: ");
                if (!int.TryParse(Console.ReadLine(), out int ageInput) || ageInput < 0 || ageInput > 100)
                {
                    OutputMessage("Please enter a valid age.");
                    return null;
                }

                Console.Write("Birthdate(MM/DD/YYYY): ");
                string bdateInput = Console.ReadLine();
                if (Regex.IsMatch(bdateInput, @"^\d{2}/\d{2}/\d{4}$"))
                {

                }
                else
                {
                    Console.WriteLine("Invalid date format. Please use MM/DD/YYYY.");
                    return null;
                }


                Console.Write("Gender(M/F): ");
                string genderInput = Console.ReadLine()?.ToUpper();
                if (genderInput != "M" && genderInput != "F")
                {
                    OutputMessage("Please enter a valid gender (M or F).");
                    return null;
                }

                Console.Write("Section: BS INFO ");
                string sectionInput = Console.ReadLine();

                if (!IsValidSection(sectionInput))
                {
                    OutputMessage("Please enter a valid section");
                    return null;
                }

                Console.Write("Student ID(0000-0000): ");
                string studentIdInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(studentIdInput) || !IsValidStudentIdFormat(studentIdInput))
                {
                    OutputMessage("Enter a valid student ID in the format 0000-0000.");
                    return null;
                }

                return new Person(nameInput, ageInput, bdateInput, genderInput, sectionInput, studentIdInput);

            }

            private bool IsValidStudentIdFormat(string studentId)
            {

                return System.Text.RegularExpressions.Regex.IsMatch(studentId, @"^\d{4}-\d{4}$");
            }

            private bool IsValidSection(string section)
            {
                List<string> allowedSections = new List<string> { "1A", "1B", "1C", "2A", "2B", "2C", "3A", "3B", "3C", "4A", "4B", "4C" };

                return allowedSections.Contains(section.ToUpper());
            }

            public bool isSystemEmpty()
            {
                if (people.Count == 0)
                {
                    Console.WriteLine("There are no students in the system.");
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        static void Main(string[] args)
        {
            new Manager();

            Console.WriteLine("ADIOS,HAVE A NICE DAY!");

            Console.ReadLine();
        }
    }
}