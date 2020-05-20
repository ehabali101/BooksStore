using BooksStore.Models;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

public class CsvImport
{
    public static List<Book> GetBooks(string fileName, string delimiters)
    {
        List<Book> result = new List<Book>();
        CultureInfo culture;

        using (TextFieldParser tfp = new TextFieldParser(fileName))
        {
            tfp.SetDelimiters(delimiters);
            tfp.ReadFields();


            // Get Remaining Rows
            while (!tfp.EndOfData)
            {
                //Title;Author;Year;Price;In Stock;Binding;Description
                culture = CultureInfo.CreateSpecificCulture("de-DE");
                string[] data = tfp.ReadFields();
                double value;
                double.TryParse(data[3], NumberStyles.Any, culture, out value);

                result.Add(new Book()
                {
                    Title = data[0],
                    Author = data[1],
                    Year = data[2],
                    Price = value,
                    InStock = data[4] == "yes" ? true : false,
                    Binding = data[5],
                    Description = data[6]
                });
            }

        }

        return result;
    }
}