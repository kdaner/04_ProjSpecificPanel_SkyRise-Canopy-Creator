using System;
using System.Windows.Forms;
using System.Collections.Generic; //for list
using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
//using MKA_MasterLibrary;
using System.Linq; // for distinct list sorting
using System.IO; //for Path




namespace SkyRise_Canopy_Creator
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
                //mapping
                static List<FamilyMappingItem> mappingTable = new List<FamilyMappingItem>();
        
        #region Interface implementation
        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref String message, Autodesk.Revit.DB.ElementSet elements)
        {
                //mapping
                mappingTable.Add(new FamilyMappingItem("W", "W-Wide Flange", @"K:\MKACADD_Hybrid\Revit\Revit Structure 2013\MKA Imperial Library\Structural\Framing\Steel\W-Wide Flange.rfa"));
                mappingTable.Add(new FamilyMappingItem("HSS", "HSS-Hollow Structural Section", @"K:\MKACADD_Hybrid\Revit\Revit Structure 2013\MKA Imperial Library\Structural\Framing\Steel\HSS-Hollow Structural Section.rfa"));
                mappingTable.Add(new FamilyMappingItem("2L", "LL-Double Angle", @"K:\MKACADD_Hybrid\Revit\Revit Structure 2013\MKA Imperial Library\Structural\Framing\Steel\LL-Double Angle.rfa"));
                mappingTable.Add(new FamilyMappingItem("CABLE", "Cable", @"J:\SkyHighMiami\Revit\Custom Revit Families\Cable.rfa"));
            
            try
            {
                Autodesk.Revit.UI.UIApplication rvtApp = commandData.Application;
                Autodesk.Revit.UI.UIDocument rvtDoc = rvtApp.ActiveUIDocument;

                //create transaction for undo capability
                Transaction tran = new Transaction(rvtDoc.Document, "SkyRise Canopy Placer");
                tran.Start();

                #region Determine if Beam Sizes are loaded
                //Autodesk.Revit.DB.FamilySymbol beamType_W14x109 = FindFamilySymbol(rvtDoc.Document, "W-Wide Flange", "W14X109");
                //if (beamType_W14x109 == null)
                //{
                //    MessageBox.Show("       The beam family is not loaded.        \n         Please load the WF family.         \n       Action Cancelled.       ", "MKA Tieback Placer 2013_v1.0");
                //    tran.Dispose();
                //    return Autodesk.Revit.UI.Result.Failed;
                //}
                //Autodesk.Revit.DB.FamilySymbol beamType_W14x120 = FindFamilySymbol(rvtDoc.Document, "W-Wide Flange", "W14X120");
                //if (beamType_W14x120 == null)
                //{
                //    MessageBox.Show("       The beam family is not loaded.        \n         Please load the WF family.         \n       Action Cancelled.       ", "MKA Tieback Placer 2013_v1.0");
                //    tran.Dispose();
                //    return Autodesk.Revit.UI.Result.Failed;
                //}
                //Autodesk.Revit.DB.FamilySymbol beamType_W14x176 = FindFamilySymbol(rvtDoc.Document, "W-Wide Flange", "W14X176");
                //if (beamType_W14x176 == null)
                //{
                //    MessageBox.Show("       The beam family is not loaded.        \n         Please load the 'Tieback (MKA)' foundation family.         \n       Action Cancelled.       ", "MKA Tieback Placer 2013_v1.0");
                //    tran.Dispose();
                //    return Autodesk.Revit.UI.Result.Failed;
                //}
                //Autodesk.Revit.DB.FamilySymbol beamType_W14x22 = FindFamilySymbol(rvtDoc.Document, "W-Wide Flange", "W14X22");
                //if (beamType_W14x22 == null)
                //{
                //    MessageBox.Show("       The beam family is not loaded.        \n         Please load the WF family.         \n       Action Cancelled.       ", "MKA Tieback Placer 2013_v1.0");
                //    tran.Dispose();
                //    return Autodesk.Revit.UI.Result.Failed;
                //}
                //Autodesk.Revit.DB.FamilySymbol beamType_W14x233 = FindFamilySymbol(rvtDoc.Document, "W-Wide Flange", "W14X233");
                //if (beamType_W14x233 == null)
                //{
                //    MessageBox.Show("       The beam family is not loaded.        \n         Please load the WF family.         \n       Action Cancelled.       ", "MKA Tieback Placer 2013_v1.0");
                //    tran.Dispose();
                //    return Autodesk.Revit.UI.Result.Failed;
                //}
                //Autodesk.Revit.DB.FamilySymbol beamType_W14x283 = FindFamilySymbol(rvtDoc.Document, "W-Wide Flange", "W14X283");
                //if (beamType_W14x283 == null)
                //{
                //    MessageBox.Show("       The beam family is not loaded.        \n         Please load the WF family.         \n       Action Cancelled.       ", "MKA Tieback Placer 2013_v1.0");
                //    tran.Dispose();
                //    return Autodesk.Revit.UI.Result.Failed;
                //}
                //Autodesk.Revit.DB.FamilySymbol beamType_W14x455 = FindFamilySymbol(rvtDoc.Document, "W-Wide Flange", "W14X455");
                //if (beamType_W14x455 == null)
                //{
                //    MessageBox.Show("       The beam family is not loaded.        \n         Please load the WF family.         \n       Action Cancelled.       ", "MKA Tieback Placer 2013_v1.0");
                //    tran.Dispose();
                //    return Autodesk.Revit.UI.Result.Failed;
                //}
                //Autodesk.Revit.DB.FamilySymbol beamType_W14x99 = FindFamilySymbol(rvtDoc.Document, "W-Wide Flange", "W14X99");
                //if (beamType_W14x99 == null)
                //{
                //    MessageBox.Show("       The beam family is not loaded.        \n         Please load the WF family.         \n       Action Cancelled.       ", "MKA Tieback Placer 2013_v1.0");
                //    tran.Dispose();
                //    return Autodesk.Revit.UI.Result.Failed;
                //}
                //Autodesk.Revit.DB.FamilySymbol beamType_CABLE8 = FindFamilySymbol(rvtDoc.Document, "Cable", "CABLE8");
                //if (beamType_CABLE8 == null)
                //{
                //    MessageBox.Show("       The beam family is not loaded.        \n         Please load the CABLE family.         \n       Action Cancelled.       ", "MKA Tieback Placer 2013_v1.0");
                //    tran.Dispose();
                //    return Autodesk.Revit.UI.Result.Failed;
                //}
                //Autodesk.Revit.DB.FamilySymbol beamType_HSS10X10X12 = FindFamilySymbol(rvtDoc.Document, "HSS-Hollow Structural Section", "HSS10X10X1/2");
                //if (beamType_HSS10X10X12 == null)
                //{
                //    MessageBox.Show("       The beam family is not loaded.        \n         Please load the HSS family.         \n       Action Cancelled.       ", "MKA Tieback Placer 2013_v1.0");
                //    tran.Dispose();
                //    return Autodesk.Revit.UI.Result.Failed;
                //}
                //Autodesk.Revit.DB.FamilySymbol beamType_HSS12X12X12 = FindFamilySymbol(rvtDoc.Document, "HSS-Hollow Structural Section", "HSS12X12X1/2");
                //if (beamType_HSS12X12X12 == null)
                //{
                //    MessageBox.Show("       The beam family is not loaded.        \n         Please load the HSS family.         \n       Action Cancelled.       ", "MKA Tieback Placer 2013_v1.0");
                //    tran.Dispose();
                //    return Autodesk.Revit.UI.Result.Failed;
                //}
                //Autodesk.Revit.DB.FamilySymbol beamType_HSS8X8X12 = FindFamilySymbol(rvtDoc.Document, "HSS-Hollow Structural Section", "HSS8X8X1/2");
                //if (beamType_HSS8X8X12 == null)
                //{
                //    MessageBox.Show("       The beam family is not loaded.        \n         Please load the HSS family.         \n       Action Cancelled.       ", "MKA Tieback Placer 2013_v1.0");
                //    tran.Dispose();
                //    return Autodesk.Revit.UI.Result.Failed;
                //}
                //Autodesk.Revit.DB.FamilySymbol beamType_2L6X6X12 = FindFamilySymbol(rvtDoc.Document, "LL-Double Angle", "2L6X6X1/2");
                //if (beamType_2L6X6X12 == null)
                //{
                //    MessageBox.Show("       The beam family is not loaded.        \n         Please load the 2L family.         \n       Action Cancelled.       ", "MKA Tieback Placer 2013_v1.0");
                //    tran.Dispose();
                //    return Autodesk.Revit.UI.Result.Failed;
                //}
                //Autodesk.Revit.DB.FamilySymbol beamType_2L8X8X12 = FindFamilySymbol(rvtDoc.Document, "LL-Double Angle", "2L8X8X1/2");
                //if (beamType_2L8X8X12 == null)
                //{
                //    MessageBox.Show("       The beam family is not loaded.        \n         Please load the 2L family.         \n       Action Cancelled.       ", "MKA Tieback Placer 2013_v1.0");
                //    tran.Dispose();
                //    return Autodesk.Revit.UI.Result.Failed;
                //}
                #endregion

                //display open file dialog
                String input = string.Empty;
                //apply restrictions to what displays in the file open dialog
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "csv files (*.csv)|*.csv";
                dialog.InitialDirectory = "C:";
                dialog.Title = "Select a .csv file";

                if (dialog.ShowDialog() == DialogResult.OK) //the user selected a csv file
                {
                    input = dialog.FileName;
                }
                if (input == String.Empty) //the user didn't select a csv file
                {
                    MessageBox.Show("       You didn't select a csv file.       \n       Action Cancelled.       ", "SkyRise Canopy Placer");
                    //kill the transaction and end the program
                    tran.Dispose();
                    return Autodesk.Revit.UI.Result.Failed;
                }

                //parse csv input and create list of strings 
                List<string[]> csvoutput = new List<string[]>();
                csvparser parser = new csvparser();
                csvoutput = parser.parsecsv(input);
                //if the csv file was empty, cancel the operation
                if (csvoutput.Count == 0)
                {
                    MessageBox.Show("       The csv file is empty.       \n       Action Cancelled.       ", "SkyRise Canopy Placer");
                    tran.Dispose();
                    return Autodesk.Revit.UI.Result.Failed;
                }

                #region debugging code
                //Awesome deugging code that displays the array contents after parsing
                //String debug = "The contents of the csvoutput are: \n";
                //int rowcount = 0;
                //foreach (string[] row in csvoutput)
                //{
                //    debug = debug + "row " + rowcount.ToString() + " : ";
                //    foreach (string cells in row)
                //    {
                //        debug = debug + "[" + cells + "] ";
                //    }
                //    debug = debug + "\n";
                //    rowcount++;
                //}
                //MessageBox.Show(debug, "Debug");
                #endregion

                List<string> distinctsizes = FindUniqueMemberSizes(csvoutput);

                loadMemberFamilies(distinctsizes, rvtDoc, mappingTable);

                loadMemberSizes(distinctsizes, rvtDoc);


                //loop through the list of strings, assign variable values and place the ties in the model
                foreach (string[] strings in csvoutput)
                {
                    string rowNum = strings[0];
                    string section = strings[1];
                    double startXcoord = Convert.ToDouble(strings[2]);
                    double startYcoord = Convert.ToDouble(strings[3]);
                    double startZcoord = Convert.ToDouble(strings[4]);
                    double endXcoord = Convert.ToDouble(strings[5]);
                    double endYcoord = Convert.ToDouble(strings[6]);
                    double endZcoord = Convert.ToDouble(strings[7]);
                    bool isTrussChord = convertStringtoBool(strings[8]);
                    bool isTrussWeb = convertStringtoBool(strings[9]);
                    bool isTopSurface = convertStringtoBool(strings[10]);
                    bool isLowerSurface = convertStringtoBool(strings[11]);

                    #region MyRegion
                    //FamilySymbol beamSectionSize = beamType_W14x22;
                    //if (section == "2L6X6X1/2")
                    //{
                    //    beamSectionSize = beamType_2L6X6X12;
                    //}
                    //if (section == "2L8X8X1/2")
                    //{
                    //    beamSectionSize = beamType_2L8X8X12;
                    //}
                    //if (section == "CABLE8")
                    //{
                    //    beamSectionSize = beamType_CABLE8;
                    //}
                    //if (section == "HSS10X10X.500")
                    //{
                    //    beamSectionSize = beamType_HSS10X10X12;
                    //}
                    //if (section == "HSS12X12X.500")
                    //{
                    //    beamSectionSize = beamType_HSS12X12X12;
                    //}
                    //if (section == "HSS8X8X.500")
                    //{
                    //    beamSectionSize = beamType_HSS8X8X12;
                    //}
                    //if (section == "W14X109")
                    //{
                    //    beamSectionSize = beamType_W14x109;
                    //}
                    //if (section == "W14X120")
                    //{
                    //    beamSectionSize = beamType_W14x120;
                    //}
                    //if (section == "W14X176")
                    //{
                    //    beamSectionSize = beamType_W14x176;
                    //}
                    //if (section == "W14X22")
                    //{
                    //    beamSectionSize = beamType_W14x22;
                    //}
                    //if (section == "W14X233")
                    //{
                    //    beamSectionSize = beamType_W14x233;
                    //}
                    //if (section == "W14X283")
                    //{
                    //    beamSectionSize = beamType_W14x283;
                    //}
                    //if (section == "W14X455")
                    //{
                    //    beamSectionSize = beamType_W14x455;
                    //}
                    //if (section == "W14X99")
                    //{
                    //    beamSectionSize = beamType_W14x99;
                    //} 
                    #endregion

               //go thru the string and get the first non digit chars
                     string memberShape = null;
                       // int z = section.TakeWhile(c => char.IsLetter(c)).Count();
                    //memberShape = section.Substring(0,z);
                     memberShape = findSectionPrefix(section);

                //check to see if the desired size is in mapping table
                FamilyMappingItem temp = mappingTable.Find(x => x.SectionShortName.Equals(memberShape));
                if(temp == null){//didn't find the section in the mapping table
                throw new Exception("can't find the section prefix " + memberShape + " in the mapping table");
                }    
                
                   FamilySymbol beamSectionSize = FindFamilySymbol(rvtDoc.Document, temp.FamilyName, section);
                   PlaceBeam(rvtApp.Application, rvtDoc.Document, beamSectionSize, rowNum, section, startXcoord, startYcoord, startZcoord, endXcoord, endYcoord, endZcoord, isTrussChord, isTrussWeb, isTopSurface, isLowerSurface, rowNum);
                    //TODO convert variables to shared parameters
                }

                //commit transaction
                tran.Commit();

                //Display confirmation dialog
                MessageBox.Show("      Placed " + csvoutput.Count.ToString() + " members.      ", "SkyRise Canopy Placer");

                //Call home to usage monitoring database
                String ribbonToolName = "SkyRise Canopy Placer";
                String ribbonToolVersion = "2014_v1.0";
                // MKA_MasterLibrary.MKAUtilities test = new MKA_MasterLibrary.MKAUtilities();
                // test.CallHome(ribbonToolName, ribbonToolVersion, rvtDoc.Document.PathName, null, null, null, null, csvoutput.Count.ToString(), 0, 0);

                //  return succeeded info. 
                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch (System.FormatException formatex)
            {
                message = "Something is wrong with the data in the csv file. Check for bank fields, missing fields, etc.\n";
                return Autodesk.Revit.UI.Result.Failed;
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                return Autodesk.Revit.UI.Result.Failed;
            }
        }
        #endregion

        private string findSectionPrefix(string input)
        {
            //go thru the string and get the first non digit chars
            string memberShape = null;
            int z = input.TakeWhile(c => char.IsLetter(c)).Count();
            memberShape = input.Substring(0, z);

            //check to see if member is a 2L which will fail the character test above
            if (input.Substring(0, 2) == "2L")
            {
                return "2L";
            }
            
            return memberShape;
        }


        /// <summary>
        /// find the tieback family that will be placed
        /// </summary>
        /// <param name="rvtDoc">Revit document</param>
        /// <param name="familyName">Family name of tieback</param>
        /// <param name="symbolName">Symbol of tieback</param>
        /// <returns></returns>
        private FamilySymbol FindFamilySymbol(Document rvtDoc, string familyName, string symbolName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(rvtDoc);
            FilteredElementIterator itr = collector.OfClass(typeof(Family)).GetElementIterator();
            itr.Reset();
            while (itr.MoveNext())
            {
                Autodesk.Revit.DB.Element elem = (Autodesk.Revit.DB.Element)itr.Current;
                if (elem.GetType() == typeof(Autodesk.Revit.DB.Family))
                {
                    if (elem.Name == familyName)
                    {
                        Autodesk.Revit.DB.Family family = (Autodesk.Revit.DB.Family)elem;
                        foreach (Autodesk.Revit.DB.FamilySymbol symbol in family.Symbols)
                        {
                            if (symbol.Name == symbolName)
                            {
                                return symbol;
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// find the tieback family that will be placed
        /// </summary>
        /// <param name="rvtDoc">Revit document</param>
        /// <param name="familyName">Family name of tieback</param>
        /// <param name="symbolName">Symbol of tieback</param>
        /// <returns></returns>
        private bool CanFindFamilySymbol(Document rvtDoc, string familyName, string symbolName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(rvtDoc);
            FilteredElementIterator itr = collector.OfClass(typeof(Family)).GetElementIterator();
            itr.Reset();
            while (itr.MoveNext())
            {
                
                Autodesk.Revit.DB.Element elem = (Autodesk.Revit.DB.Element)itr.Current;
                if (elem.GetType() == typeof(Autodesk.Revit.DB.Family))
                {
                    if (elem.Name == familyName)
                    {
                        //MessageBox.Show("if family name: " + familyName + " symbol name: " + symbolName);
                        Autodesk.Revit.DB.Family family = (Autodesk.Revit.DB.Family)elem;
                        foreach (Autodesk.Revit.DB.FamilySymbol symbol in family.Symbols)
                        {
                           // MessageBox.Show("foreach family name: " + family.Name + " symbol name: " + symbol.Name);
                            if (symbol.Name == symbolName)
                            {
                             
                                return true;
                            }
                        }
                    }
                }
            }
          //  MessageBox.Show("can find returning false");
            return false;
        }

        /// <summary>
        /// find the tieback family that will be placed
        /// </summary>
        /// <param name="rvtDoc">Revit document</param>
        /// <param name="familyName">Family name of tieback</param>
        /// <param name="symbolName">Symbol of tieback</param>
        /// <returns></returns>
        private bool FindFamily(Document rvtDoc, string familyName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(rvtDoc);
            FilteredElementIterator itr = collector.OfClass(typeof(Family)).GetElementIterator();
            itr.Reset();
            while (itr.MoveNext())
            {
                Autodesk.Revit.DB.Element elem = (Autodesk.Revit.DB.Element)itr.Current;
                if (elem.GetType() == typeof(Autodesk.Revit.DB.Family))
                {
                    if (elem.Name == familyName)
                    {
                        //Autodesk.Revit.DB.Family family = (Autodesk.Revit.DB.Family)elem;
                        //foreach (Autodesk.Revit.DB.FamilySymbol symbol in family.Symbols)
                        //{
                        //    if (symbol.Name == symbolName)
                        //    {
                        //        return symbol;
                        //    }
                        //}
                        return true;
                    }
                }
            }
            return false;
        }

        private void PlaceBeam(Autodesk.Revit.ApplicationServices.Application rvtApp, Document rvtDoc, FamilySymbol beamType, string rowNum, string section, double startx, double starty, double startz, double endx, double endy, double endz, bool chord, bool web, bool top, bool lower, string rowid)
        {
            Autodesk.Revit.DB.XYZ point = new Autodesk.Revit.DB.XYZ(startx, starty, startz);

            XYZ start = new XYZ(startx, starty, startz);
            XYZ end = new XYZ(endx, endy, endz);
            Line line = rvtApp.Create.NewLineBound(start, end);
            Autodesk.Revit.DB.FamilyInstance beam;

            //if the beam is a web member, draw it as a brace
            if (web == true)
            {
                beam = rvtDoc.Create.NewFamilyInstance(line, beamType, null, Autodesk.Revit.DB.Structure.StructuralType.Brace);
                if (beam == null)
                {
                    MessageBox.Show("Error. Failed to create an instance of a brace.");
                    return;
                }
            }
            else
            {  //else draw it as a beam
                beam = rvtDoc.Create.NewFamilyInstance(line, beamType, null, Autodesk.Revit.DB.Structure.StructuralType.Beam);
                if (beam == null)
                {
                    MessageBox.Show("Error. Failed to create an instance of a beam.");
                    return;
                }
            }

            //set parameter values
            //double angle2 = angle;
            //Autodesk.Revit.DB.XYZ zVec = new Autodesk.Revit.DB.XYZ(0, 0, 1);
            //Autodesk.Revit.DB.Line axis = rvtApp.Create.NewLineUnbound(point, zVec);
            //tieback.Location.Rotate(axis, angle2);

            ParameterSet tiebackparams = beam.Parameters;
            foreach (Parameter p2 in tiebackparams)
            {
                //TODO add paramters to create schedule of tieback information
                if (p2.Definition.Name.ToString() == "Canopy_isTrussChord")
                {
                    int result = 0;
                    if (chord == true) { result = 1; }
                    p2.Set(result);
                }
                if (p2.Definition.Name.ToString() == "Canopy_isTrussWeb")
                {
                    int result = 0;
                    if (web == true) { result = 1; }
                    p2.Set(result);
                }
                if (p2.Definition.Name.ToString() == "Canopy_isUpperSurface")
                {
                    int result = 0;
                    if (top == true) { result = 1; }
                    p2.Set(result);
                }
                if (p2.Definition.Name.ToString() == "Canopy_isLowerSurface")
                {
                    int result = 0;
                    if (lower == true) { result = 1; }
                    p2.Set(result);
                }
                if (p2.Definition.Name.ToString() == "Comments")
                {
                    p2.Set("row id: " + rowid);
                }
            }
        }

        //convert strings to booleans
        private bool convertStringtoBool(string input)
        {
            if (input.ToUpper() != "TRUE" && input.ToUpper() != "FALSE")
            {
                throw new Exception("invalid boolean value");
            }
            if (input.ToUpper() == "TRUE")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //read through csv output and get the list of distinct sizes
        private List<string> FindUniqueMemberSizes(List<string[]> csvoutput)
        {
            List<string> allSizes = new List<string>();

            foreach (string[] strings in csvoutput)
            {
                string rowNum = strings[0];
                string section = strings[1];
                allSizes.Add(section);
            }
            List<string> distinctSizes = allSizes.Distinct().ToList();
            return distinctSizes;
        }

        //class for mapping
        class FamilyMappingItem
        {
            public string SectionShortName { get; set; }
            public string FamilyName { get; set; }
            public string FamilyPath { get; set; }

            public FamilyMappingItem(string shortname, string name, string path)
            {
                SectionShortName = shortname;
                FamilyName = name;
                FamilyPath = path;
            }

        }
        
        //go through the sizes list and load  families if necessary
        private bool loadMemberFamilies(List<string> membersToLoad, Autodesk.Revit.UI.UIDocument rvtDoc, List<FamilyMappingItem> mappingTable)
        {
            foreach (FamilyMappingItem fmi in mappingTable)
            {
                //http://stackoverflow.com/questions/2277444/how-to-check-if-any-words-in-a-list-contain-a-partial-string
                //if size contains the short name
                if (membersToLoad.Any(l => l.Contains(fmi.SectionShortName)))
                {
                    //check to see if family is loaded
                    FamilyMappingItem temp = mappingTable.Find(x => x.SectionShortName.Equals(fmi.SectionShortName));
                    if (FindFamily(rvtDoc.Document, temp.FamilyName))
                    {
                        MessageBox.Show(temp.FamilyName.ToString() + " family already loaded");
                    }
                    else // the  family isn't loaded so load
                    {
                        bool loadSuccess = rvtDoc.Document.LoadFamily(temp.FamilyPath);
                        if (loadSuccess)
                        {
                            MessageBox.Show("Loaded the " + fmi.FamilyName +" family ok.");
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //go through the sizes list and load member sizes if necessary
        private bool loadMemberSizes(List<string> membersToLoad, Autodesk.Revit.UI.UIDocument rvtDoc)
        {
            foreach (string size in membersToLoad)
            {
                //get the size string
                //DONE
                
                //go thru the string and get the first non digit chars
                string memberShape = null;
                    //int z = size.TakeWhile(c => char.IsLetter(c)).Count();
                    //memberShape = size.Substring(0,z);

                memberShape = findSectionPrefix(size);

              // MessageBox.Show(" membershape: "+ memberShape);

                //check to see if the desired size is in mapping table
                FamilyMappingItem temp = mappingTable.Find(x => x.SectionShortName.Equals(memberShape));
                if(temp == null){//didn't find the section in the mapping table
                throw new Exception("can't find the section prefix " + memberShape + " in the mapping table");
                }    
                
                //try to load the symbol
                if (CanFindFamilySymbol(rvtDoc.Document, temp.FamilyName, size))
                    {
                       // MessageBox.Show(temp.FamilyName.ToString() + " member size already loaded");
                    }
                    else // the  member size isn't loaded so load
                    {
                        //create the family load options object
                 //SampleFamilyLoadOptions loadoptions = new SampleFamilyLoadOptions();

                        FamilySymbol resultSymbol;
                    
                    
                    //MessageBox.Show("family is not loaded temp.familypath: " + temp.FamilyPath + " size: " + size);
                        bool loadSuccess = rvtDoc.Document.LoadFamilySymbol(temp.FamilyPath, size, new SampleFamilyLoadOptions(), out resultSymbol);
                       
                    
                    // MessageBox.Show("full file path: "+ Path.GetFullPath(@"K:\MKACADD_Hybrid\Revit\Revit Structure 2013\MKA Imperial Library\Structural\Framing\Steel\W-Wide Flange.rfa").ToString());
                       // bool loadSuccess = rvtDoc.Document.LoadFamilySymbol(Path.GetFullPath(@"K:\MKACADD_Hybrid\Revit\Revit Structure 2013\MKA Imperial Library\Structural\Framing\Steel\W-Wide Flange.rfa"), "W12X26");
                  //  MessageBox.Show("loadsuccess: " + loadSuccess.ToString());
                        if (loadSuccess == true)
                        {
                 //           MessageBox.Show("Loaded the " + temp.FamilyName +" : " + size + " symbol ok.");
                        }
                        else
                        {
                    //        MessageBox.Show("load member sizes returning false");
                            return false;
                        }
                    }
                }
            return true;
        }

        //  Helper function - find the given file name in the set of Revit library paths. 
        public string findFile(Autodesk.Revit.ApplicationServices.Application rvtApp, string fileName)
        {

            IDictionary<string, string> paths = rvtApp.GetLibraryPaths();
            IEnumerator<KeyValuePair<string, string>> iter = paths.GetEnumerator();
            string filePath = null;
            // loop through each path in the collection. 

            while ((iter.MoveNext()))
            {
                string path = iter.Current.Value;
                //libPaths += ControlChars.Tab + path + ";" + ControlChars.NewLine;
                filePath = SearchFile(path, fileName);
                if ((filePath != null))
                {
                    return filePath;
                }

            }
            filePath = SearchFile(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), fileName);
            if ((filePath != null))
            {
                return filePath;
            }
            return null;
        }

        //  Helper function - recursively search the given file name under the current directory. 
        public string SearchFile(string path, string fileName)
        {
            if (!Directory.Exists(path))
            {
                return null;
            }
            //  search this directory 
            string fname = null;
            foreach (string fname_loopVariable in Directory.GetFiles(path, fileName))
            {
                fname = fname_loopVariable;
                MessageBox.Show("I found the file in: " + fname);
                return path;
            }

            //  recursively search child directories.  
            string dname = null;
            foreach (string dname_loopVariable in Directory.GetDirectories(path))
            {
                dname = dname_loopVariable;
                string filePath = SearchFile(dname, fileName);
                if ((filePath != null))
                {
                    return filePath;
                }
            }

            return null;
        }
    }


}

public class SampleFamilyLoadOptions : IFamilyLoadOptions
{
    #region IFamilyLoadOptions Members

    public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
    {
        if (!familyInUse)
        {
           // TaskDialog.Show("SampleFamilyLoadOptions", "The family has not been in use and will keep loading.");

            overwriteParameterValues = true;
            return true;
        }
        else
        {
           // TaskDialog.Show("SampleFamilyLoadOptions", "The family has been in use but will still be loaded with existing parameters overwritten.");

            overwriteParameterValues = false;
            return true;
        }
    }

    public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
    {
        if (!familyInUse)
        {
           // TaskDialog.Show("SampleFamilyLoadOptions", "The shared family has not been in use and will keep loading.");

            source = FamilySource.Family;
            overwriteParameterValues = true;
            return true;
        }
        else
        {
           // TaskDialog.Show("SampleFamilyLoadOptions", "The shared family has been in use but will still be loaded from the FamilySource with existing parameters overwritten.");

            source = FamilySource.Family;
            overwriteParameterValues = false;
            return true;
        }
    }

    #endregion
}
