using System.Drawing;

Bitmap inputImage = new Bitmap("input.jpg");

CalculateAverageColor(inputImage);
CalculateContrast(inputImage);
CalculateColorVariance(inputImage);
CalculateIntensity(inputImage);

static void CalculateAverageColor(Bitmap image)
{
    long sumR = 0, sumG = 0, sumB = 0;
    int totalPixels = image.Width * image.Height;

    for (int x = 0; x < image.Width; x++)
    {
        for (int y = 0; y < image.Height; y++)
        {
            Color pixel = image.GetPixel(x, y);
            sumR += pixel.R;
            sumG += pixel.G;
            sumB += pixel.B;
        }
    }

    int avgR = (int)(sumR / totalPixels);
    int avgG = (int)(sumG / totalPixels);
    int avgB = (int)(sumB / totalPixels);

    Console.WriteLine($"Average Color (RGB): ({avgR}, {avgG}, {avgB})");
}

static void CalculateContrast(Bitmap image)
{
    double sumBrightness = 0, sumBrightnessSq = 0;
    int totalPixels = image.Width * image.Height;

    for (int x = 0; x < image.Width; x++)
    {
        for (int y = 0; y < image.Height; y++)
        {
            Color pixel = image.GetPixel(x, y);
            float brightness = pixel.GetBrightness();
            sumBrightness += brightness;
            sumBrightnessSq += brightness * brightness;
        }
    }

    double meanBrightness = sumBrightness / totalPixels;
    double variance = (sumBrightnessSq / totalPixels) - (meanBrightness * meanBrightness);
    double contrast = Math.Sqrt(variance);

    Console.WriteLine($"Contrast: {contrast}");
}

static void CalculateColorVariance(Bitmap image)
{
    long sumR = 0, sumG = 0, sumB = 0;
    long sumRsq = 0, sumGsq = 0, sumBsq = 0;
    int totalPixels = image.Width * image.Height;

    for (int x = 0; x < image.Width; x++)
    {
        for (int y = 0; y < image.Height; y++)
        {
            Color pixel = image.GetPixel(x, y);
            sumR += pixel.R;
            sumG += pixel.G;
            sumB += pixel.B;
            sumRsq += pixel.R * pixel.R;
            sumGsq += pixel.G * pixel.G;
            sumBsq += pixel.B * pixel.B;
        }
    }

    double avgR = (double)sumR / totalPixels;
    double avgG = (double)sumG / totalPixels;
    double avgB = (double)sumB / totalPixels;

    double varR = (double)sumRsq / totalPixels - avgR * avgR;
    double varG = (double)sumGsq / totalPixels - avgG * avgG;
    double varB = (double)sumBsq / totalPixels - avgB * avgB;

    Console.WriteLine($"Color Variance - Red: {varR}, Green: {varG}, Blue: {varB}");
}

static void CalculateIntensity(Bitmap image)
{
    long sumIntensity = 0;
    int totalPixels = image.Width * image.Height;

    for (int x = 0; x < image.Width; x++)
    {
        for (int y = 0; y < image.Height; y++)
        {
            Color pixel = image.GetPixel(x, y);
            // محاسبه مقدار خاکستری
            int gray = (int)(0.3 * pixel.R + 0.59 * pixel.G + 0.11 * pixel.B);
            sumIntensity += gray;
        }
    }

    double avgIntensity = (double)sumIntensity / totalPixels;
    Console.WriteLine($"Average Intensity: {avgIntensity}");
}
