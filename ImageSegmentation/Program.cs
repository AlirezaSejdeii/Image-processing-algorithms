using System.Drawing;

Bitmap originalImage = new Bitmap("input.jpg");

Bitmap segmentedImage = new Bitmap(originalImage.Width, originalImage.Height);

int minRed = 100, maxRed = 255;
int minGreen = 0, maxGreen = 100;
int minBlue = 0, maxBlue = 100;

for (int x = 0; x < originalImage.Width; x++)
{
    for (int y = 0; y < originalImage.Height; y++)
    {
        Color pixelColor = originalImage.GetPixel(x, y);

        if (pixelColor.R >= minRed && pixelColor.R <= maxRed &&
            pixelColor.G >= minGreen && pixelColor.G <= maxGreen &&
            pixelColor.B >= minBlue && pixelColor.B <= maxBlue)
        {
            segmentedImage.SetPixel(x, y, pixelColor);
        }
        else
        {
            segmentedImage.SetPixel(x, y, Color.White);
        }
    }
}

segmentedImage.Save("color-segmentation-output.jpg");