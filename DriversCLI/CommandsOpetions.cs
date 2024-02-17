using CommandLine;
using System.Runtime.InteropServices;

namespace DriversCLI
{
    public class CommandsOptions
    {
        [Verb("get", HelpText = "Retrieve drivers data. Include ID for a specific driver.")]
        public class GetOptions
        {
            [Option('i', "id", Required = false, HelpText = "The ID to retrieve.")]
            public int? Id { get; set; }
        }

        [Verb("add", HelpText = "Add a fake driver.")]
        public class AddOptions
        { }

        [Verb("update", HelpText = "Update an existing driver.")]
        public class UpdateOptions
        {
            [Option('i', "id", Required = true, HelpText = "The ID to update.")]
            public int Id { get; set; }

            [Option('f', "firstname", Required = true, HelpText = "The new first name.")]
            public string FirstName { get; set; }

            [Option('l', "lastname", Required = true, HelpText = "The new last name.")]
            public string LastName { get; set; }

            [Option('e', "email", Required = false, HelpText = "The new email.")]
            public string Email { get; set; }

            [Option('p', "phonenumber", Required = false, HelpText = "The new phone number.")]
            public string PhoneNumber { get; set; }
        }

        [Verb("delete", HelpText = "Delete a driver by ID.")]
        public class DeleteOptions
        {
            [Option('i', "id", Required = true, HelpText = "The ID of the driver to delete.")]
            public int Id { get; set; }
        }

        [Verb("fake", HelpText = "Adds fake drivers. Default value for count is 10.")]
        public class AddFakeOptions
        {
            [Option('c', "count", Required = false, HelpText = "The ID to retrieve.")]
            public int Count { get; set; } = 10;
        }

        [Verb("alphabetize", HelpText = "Retrieve alphabetized drivers' names. Include ID for a specific driver.")]
        public class GetAlphabetizeOptions
        {
            [Option('i', "id", Required = false, HelpText = "The ID of the driver to alphabetize.")]
            public int? Id { get; set; }
        }
    }
}
