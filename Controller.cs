using System;
using System.Windows.Forms;
using System.Collections.Generic; //for list
using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using MKA_MasterLibrary;//for call home

namespace SkyRise_Canopy_Creator
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
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
            try
            {
                Autodesk.Revit.UI.UIApplication rvtApp = commandData.Application;
                Autodesk.Revit.UI.UIDocument rvtDoc = rvtApp.ActiveUIDocument;

                //create transaction for undo capability
                Transaction tran = new Transaction(rvtDoc.Document, "SkyRise Canopy Placer");
                tran.Start();

                Autodesk.Revit.DB.FamilySymbol beamType_W14x90 = FindFamilySymbol(rvtDoc.Document, "W-Wide Flange", "W14X90");
                if (beamType_W14x90 == null)
                {
                    MessageBox.Show("       The beam family is not loaded.        \n         Please load the 'Tieback (MKA)' foundation family.         \n       Action Cancelled.       ", "MKA Tieback Placer 2013_v1.0");
                    tran.Dispose();
                    return Autodesk.Revit.UI.Result.Failed;
                }

                Autodesk.Revit.DB.FamilySymbol beamType_W14x53 = FindFamilySymbol(rvtDoc.Document, "W-Wide Flange", "W14X53");
                if (beamType_W14x53 == null)
                {
                    MessageBox.Show("       The beam family is not loaded.        \n         Please load the 'Tieback (MKA)' foundation family.         \n       Action Cancelled.       ", "MKA Tieback Placer 2013_v1.0");
                    tran.Dispose();
                    return Autodesk.Revit.UI.Result.Failed;
                }

                Autodesk.Revit.DB.FamilySymbol beamType_W14x22 = FindFamilySymbol(rvtDoc.Document, "W-Wide Flange", "W14X22");
                if (beamType_W14x22 == null)
                {
                    MessageBox.Show("       The beam family is not loaded.        \n         Please load the 'Tieback (MKA)' foundation family.         \n       Action Cancelled.       ", "MKA Tieback Placer 2013_v1.0");
                    tran.Dispose();
                    return Autodesk.Revit.UI.Result.Failed;
                }

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

                    FamilySymbol beamSectionSize = beamType_W14x22;
                    if (section == "W14x22")
            {
                beamSectionSize = beamType_W14x22;
            }
                    if (section == "W14x53")
                    {
                        beamSectionSize = beamType_W14x53;
                    }
                    if (section == "W14x90")
                    {
                        beamSectionSize = beamType_W14x90;
                    }

                    //Autodesk.Revit.DB.XYZ placementPt = new XYZ(xcoord, ycoord, zcoord);
                    PlaceBeam(rvtApp.Application, rvtDoc.Document,beamSectionSize ,rowNum, section, startXcoord, startYcoord, startZcoord, endXcoord, endYcoord, endZcoord);
                    //TODO convert variables to shared parameters
                }

                //commit transaction
                tran.Commit();

                //Display confirmation dialog
                MessageBox.Show("      Placed " + csvoutput.Count.ToString() + " members.      ", "SkyRise Canopy Placer");

                //Call home to usage monitoring database
                String ribbonToolName = "SkyRise Canopy Placer";
                String ribbonToolVersion = "2014_v1.0";
                MKAUtilities test = new MKA_MasterLibrary.MKAUtilities();
                test.CallHome(ribbonToolName, ribbonToolVersion, rvtDoc.Document.PathName, null, null, null, null, csvoutput.Count.ToString(), 0, 0);

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

        private void PlaceBeam(Autodesk.Revit.ApplicationServices.Application rvtApp, Document rvtDoc, FamilySymbol beamType, string rowNum, string section, double startx, double starty, double startz, double endx, double endy, double endz)
        {
            Autodesk.Revit.DB.XYZ point = new Autodesk.Revit.DB.XYZ(startx, starty, startz);

            XYZ start = new XYZ(startx, starty,startz);
            XYZ end = new XYZ(endx, endy, endz);
            Line line = rvtApp.Create.NewLineBound(start, end);

            Autodesk.Revit.DB.FamilyInstance beam = rvtDoc.Create.NewFamilyInstance(line, beamType, null, Autodesk.Revit.DB.Structure.StructuralType.Beam);
            if (beam == null)
            {
                MessageBox.Show("Error. Failed to create an instance of a beam.");
                return;
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
                if (p2.Definition.Name.ToString() == "S_TiebackPlacer_X")
                {
                    p2.Set(point.X);
                }
                if (p2.Definition.Name.ToString() == "S_TiebackPlacer_Y")
                {
                    p2.Set(point.Y);
                }
                if (p2.Definition.Name.ToString() == "S_TiebackPlacer_Z")
                {
                    p2.Set(point.Z);
                }
            }
        }
     }
}
