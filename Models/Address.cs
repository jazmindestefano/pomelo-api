using System;
namespace PomeloAPI.Models
{
	public class Address
	{
        public string street_name { get; set; } = String.Empty;

        public int street_number { get; set; } = 0;

        public int floor { get; set; } = 0;

        public string apartmnet { get; set; } = String.Empty;

        public int zip_code { get; set; } = 0;

        public string neighborhood { get; set; } = String.Empty;

        public string city { get; set; } = String.Empty;

        public string region { get; set; } = String.Empty;

        public string additional_info { get; set; } = String.Empty;

        public string country { get; set; } = String.Empty;


    }
}

