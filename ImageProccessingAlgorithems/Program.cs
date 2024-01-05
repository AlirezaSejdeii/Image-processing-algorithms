using System.Drawing;

Bitmap inputImage = new Bitmap("input.jpg");

Bitmap adjustedImage = AdjustBrightnessContrast(inputImage, 1.2f, 10);
adjustedImage.Save("BrightnessContrast.jpg");

Bitmap convertRgbToHsvImage = ConvertRgBtoHsv(inputImage);
convertRgbToHsvImage.Save("RGBtoHSV.jpg");

Bitmap filteredImage = ApplyMeanFilter(inputImage, 5);
filteredImage.Save("MeanFilter.jpg");

Bitmap edgeDetectedImage = SobelFilter(inputImage);
edgeDetectedImage.Save("SobelEdgeDetection.jpg");


static Bitmap AdjustBrightnessContrast(Bitmap image, float brightness, float contrast)
{
    Bitmap adjustedImage = new Bitmap(image.Width, image.Height);

    float adjustedBrightness = brightness - 1.0f;
    Color pixel;

    for (int i = 0; i < image.Width; i++)
    {
        for (int j = 0; j < image.Height; j++)
        {
            pixel = image.GetPixel(i, j);
            float red = pixel.R * contrast + adjustedBrightness;
            float green = pixel.G * contrast + adjustedBrightness;
            float blue = pixel.B * contrast + adjustedBrightness;

            adjustedImage.SetPixel(i, j, Color.FromArgb(
                pixel.A,
                (int)Math.Max(Math.Min(255, red), 0),
                (int)Math.Max(Math.Min(255, green), 0),
                (int)Math.Max(Math.Min(255, blue), 0)
            ));
        }
    }

    return adjustedImage;
}

static Bitmap ConvertRgBtoHsv(Bitmap rgbImage)
{
    Bitmap hsvImage = new Bitmap(rgbImage.Width, rgbImage.Height);

    for (int i = 0; i < rgbImage.Width; i++)
    {
        for (int j = 0; j < rgbImage.Height; j++)
        {
            Color rgbColor = rgbImage.GetPixel(i, j);
            double r = rgbColor.R / 255.0;
            double g = rgbColor.G / 255.0;
            double b = rgbColor.B / 255.0;

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double delta = max - min;

            double h = 0;
            if (delta != 0)
            {
                if (max == r)
                {
                    h = (g - b) / delta + (g < b ? 6 : 0);
                }
                else if (max == g)
                {
                    h = (b - r) / delta + 2;
                }
                else if (max == b)
                {
                    h = (r - g) / delta + 4;
                }

                h /= 6;
            }

            double s = max == 0 ? 0 : delta / max;
            double v = max;

            int hue = (int)(h * 255);
            int saturation = (int)(s * 255);
            int value = (int)(v * 255);

            hsvImage.SetPixel(i, j, Color.FromArgb(hue, saturation, value));
        }
    }

    return hsvImage;
}

static Bitmap ApplyMeanFilter(Bitmap image, int windowSize)
{
    Bitmap filteredImage = new Bitmap(image.Width, image.Height);
    int offset = windowSize / 2;

    for (int i = offset; i < image.Width - offset; i++)
    {
        for (int j = offset; j < image.Height - offset; j++)
        {
            float sumR = 0, sumG = 0, sumB = 0;

            for (int x = -offset; x <= offset; x++)
            {
                for (int y = -offset; y <= offset; y++)
                {
                    Color pixel = image.GetPixel(i + x, j + y);
                    sumR += pixel.R;
                    sumG += pixel.G;
                    sumB += pixel.B;
                }
            }

            int count = windowSize * windowSize;
            Color newColor = Color.FromArgb(
                (int)(sumR / count),
                (int)(sumG / count),
                (int)(sumB / count)
            );

            filteredImage.SetPixel(i, j, newColor);
        }
    }

    return filteredImage;
}

static Bitmap SobelFilter(Bitmap input)
{
    Bitmap output = new Bitmap(input.Width, input.Height);
    int[,] sobelXKernel = { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
    int[,] sobelYKernel = { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };
    for (int i = 1; i < input.Width - 1; i++)
    {
        for (int j = 1; j < input.Height - 1; j++)
        {
            int? gx = sobelXKernel != null ? ApplyKernel(input, sobelXKernel, i, j) : null;
            int? gy = sobelYKernel != null ? ApplyKernel(input, sobelYKernel, i, j) : null;

            int gradientMagnitude = (int)Math.Sqrt((gx ?? 0) * (gx ?? 0) + (gy ?? 0) * (gy ?? 0));
            gradientMagnitude = Math.Min(255, Math.Max(0, gradientMagnitude));
            output.SetPixel(i, j, Color.FromArgb(gradientMagnitude, gradientMagnitude, gradientMagnitude));
        }
    }

    return output;
}

static int ApplyKernel(Bitmap input, int[,] kernel, int x, int y)
{
    int kernelWidth = kernel.GetLength(0);
    int kernelHeight = kernel.GetLength(1);
    int halfKernelWidth = kernelWidth / 2;
    int halfKernelHeight = kernelHeight / 2;
    int kernelSum = 0;

    for (int i = 0; i < kernelWidth; i++)
    {
        for (int j = 0; j < kernelHeight; j++)
        {
            int pixelX = x + (i - halfKernelWidth);
            int pixelY = y + (j - halfKernelHeight);

            if (pixelX < 0 || pixelY < 0 || pixelX >= input.Width || pixelY >= input.Height)
            {
                continue;
            }

            Color pixelColor = input.GetPixel(pixelX, pixelY);
            int grayValue = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
            kernelSum += grayValue * kernel[i, j];
        }
    }

    return kernelSum;
}