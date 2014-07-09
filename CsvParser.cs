using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//for parser
using Microsoft.VisualBasic.FileIO;

namespace SkyRise_Canopy_Creator
{
    public class csvparser
    {

        public List<string[]> parsecsv(string path)
        {
            List<string[]> parsedData = new List<string[]>();
            string[] fields;

            try
            {
                //TODO add check to see if the csv file is a valid one or validate data in each field
                TextFieldParser parser = new TextFieldParser(path);
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.TrimWhiteSpace = true;//trims white space from the strings
                int numExcludedRows = 0;

                while (!parser.EndOfData)
                {
                    fields = parser.ReadFields();

                    //check to see if the row has any blank fields
                    if (fields.Contains(""))
                    {
                        //do nothing with the string
                        numExcludedRows++;
                    }
                    else //the string doesn't include blank fields
                    {
                                parsedData.Add(fields);
                    }
                }

                if (numExcludedRows > 0)
                {
                    MessageBox.Show("Some rows were incomplete, removed data for " + numExcludedRows.ToString() + " ties.", "SkyRise Canopy Creator");
                }

                //remove the headers from the List
                if (parsedData.Count() >= 2)
                {
                    parsedData.RemoveAt(0);
                }

                //close the reader
                parser.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return parsedData;
        }

    }
}
