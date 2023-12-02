
using System.Drawing.Imaging;
using System.Drawing;
using FastReport;
using CS_Test.DTOs;

namespace CS_Test.Services
{
    public class ChartService : IChartService
    {



        // Method used to generate png resource containing pie chart
        public byte[] GenerateEmployeePieChart(List<EmployeeDTO> employees)
        {
            // calculate pie split percentages 
            var chartData = CalculatePercentage(employees);

            // Generate pie chart image
            return GeneratePieChartWithLegend(chartData);

        }



        static byte[] GeneratePieChartWithLegend(Dictionary<string, double> data)
        {
            // Create a 2D array of bytes to draw pie chart on
            using Bitmap bitmap = new(700, 500);
            
            // Create a graphics object for drawing
            using Graphics graphics = Graphics.FromImage(bitmap);

            // Clear the background
            graphics.Clear(Color.White);

            // Generate random colors for the pie slices
            Color[] sliceColors = GenerateRandomColors(data.Count);

            int colorIndex = 0;     // Color iteration index
            float startAngle = -90; // Start angle at -90 degrees (top of the circle)

            // Calculate total percentage value
            double totalValue = data.Values.Sum();

            // Set up the rectangle for the pie chart start(x=50, y=50) dimensions(400, 400)
            Rectangle pieRectangle = new(50, 50, 400, 400);

            // Draw each pie slice and legend
            foreach (var k_v in data)
            {
                double sliceAngle = (k_v.Value / totalValue) * 360; // How many degrees has the angle of current pie slice 

                using (SolidBrush brush = new(sliceColors[colorIndex]))
                {
                    graphics.FillPie(brush, pieRectangle, startAngle, (float)sliceAngle); // Drawing the pie slice using provided FillPie() graphic function
                }

                // Draw legend entry (color box,full name label and contribution percentage)
                DrawLegendEntry(graphics, Math.Round(k_v.Value, 3) + "% " + k_v.Key, sliceColors[colorIndex], colorIndex);

                startAngle += (float)sliceAngle;

                // Mod incremented value to have index out of bound safety
                colorIndex = (colorIndex + 1) % sliceColors.Length;
            }

            // Save the image to a memory stream
            using MemoryStream stream = new();

            // Serializing the array of bytes as a png resource 
            bitmap.Save(stream, ImageFormat.Png);
            
            // Return data as bytes
            return stream.ToArray();
        }




        // Helper method to draw a legend entry
        private static void DrawLegendEntry(Graphics graphics, string label, Color color, int entryIndex)
        {
            int legendX = 500; // X-coordinate for the legend
            int legendY = 50 + entryIndex * 20; // Y-coordinate for the legend entry

            // Draw color box
            Rectangle legendColorBox = new Rectangle(legendX, legendY, 15, 15);
            using (SolidBrush brush = new SolidBrush(color))
            {
                graphics.FillRectangle(brush, legendColorBox);
            }

            // Draw label
            Font font = new("Arial", 10);
            PointF labelPosition = new(legendX + 20, legendY);
            graphics.DrawString(label, font, Brushes.Black, labelPosition);
        }




        // Helper method to generate random colors
        private static Color[] GenerateRandomColors(int count)
        {
            Random random = new Random();
            Color[] colors = new Color[count];

            for (int i = 0; i < count; i++)
            {
                // Generate random color
                colors[i] = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            }

            return colors;
        
        }




        public static Dictionary<string, double> CalculatePercentage(List<EmployeeDTO> employees)
        {
            if (employees == null || employees.Count == 0)
            {
                throw new ArgumentException("Employee list is null or empty.");
            }

            // Calculate the total sum of working hours
            double totalWorkHours = employees.Sum(e => e.TotalWorkHours);

            // Create a dictionary to store the result
            Dictionary<string, double> percentageMap = new Dictionary<string, double>();

            // Calculate the percentage for each employee
            foreach (var employee in employees)
            {
                    double percentage = (employee.TotalWorkHours / totalWorkHours) * 100.0;
                    percentageMap.Add(employee.FullName ?? "Unknown", percentage);
            }

            return percentageMap;
        }




    }
}
