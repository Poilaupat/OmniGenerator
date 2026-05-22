using Barcoder;
using Barcoder.Aztec;
using Barcoder.Code128;
using Barcoder.Code39;
using Barcoder.Code93;
using Barcoder.DataMatrix;
using Barcoder.Ean;
using Barcoder.Kix;
using Barcoder.Pdf417;
using Barcoder.Qr;
using Barcoder.RoyalMail;
using Barcoder.TwoToFive;
using Barcoder.UpcA;
using Barcoder.UpcE;

namespace OmniGenerator.Lib.Renderers
{
    /// <summary>
    /// Enumerates the supported barcode types for rendering.
    /// Includes both 1D linear barcodes (Code128, EAN, UPC) and 2D matrix codes (QR Code, DataMatrix, Aztec).
    /// </summary>
    public enum EBarCodeType
    {
        /// <summary>
        /// Interleaved 2 of 5 (ITF) barcode, commonly used for logistics and warehouse applications.
        /// </summary>
        TwoToFive,

        /// <summary>
        /// Aztec Code, a 2D matrix barcode that can encode large amounts of data in a compact space.
        /// </summary>
        Aztec,

        /// <summary>
        /// Code 39, a variable-length alphanumeric barcode widely used in industrial applications.
        /// </summary>
        Code39,

        /// <summary>
        /// Code 93, a higher-density alphanumeric barcode with improved data security.
        /// </summary>
        Code93,

        /// <summary>
        /// Code 128, a high-density linear barcode capable of encoding the full ASCII character set.
        /// </summary>
        Code128,

        /// <summary>
        /// Code 128 with GS1 mode enabled for supply chain and retail applications.
        /// </summary>
        Code128GS1,

        /// <summary>
        /// Data Matrix ECC200, a 2D matrix barcode with error correction, commonly used for small item marking.
        /// </summary>
        DataMatrixECC200,

        /// <summary>
        /// Data Matrix with GS1 mode for supply chain applications.
        /// </summary>
        DataMatrixGS1,

        /// <summary>
        /// EAN (European Article Number) barcode, used for retail product identification.
        /// Supports EAN-8 and EAN-13 formats.
        /// </summary>
        EAN,

        /// <summary>
        /// KIX (Klant Index) code, used by Dutch postal services.
        /// </summary>
        KIX,

        /// <summary>
        /// PDF417, a stacked linear barcode capable of storing large amounts of data.
        /// </summary>
        PDF417,

        /// <summary>
        /// QR Code (Quick Response Code), a 2D matrix barcode widely used for mobile applications.
        /// </summary>
        QRCode,

        /// <summary>
        /// RM4SCC (Royal Mail 4-State Customer Code), used by UK postal services.
        /// </summary>
        RM4SC,

        /// <summary>
        /// UPC-A (Universal Product Code), a 12-digit barcode used primarily in North American retail.
        /// </summary>
        UPCA,

        /// <summary>
        /// UPC-E, a compact 8-digit version of UPC-A for small packages.
        /// </summary>
        UPCE,
    }

    /// <summary>
    /// Provides helper methods for generating barcode images in SVG format.
    /// Supports a wide range of 1D and 2D barcode types using the Barcoder library.
    /// </summary>
    internal static class BarCodeRenderHelper
    {
        /// <summary>
        /// Generates a barcode as an SVG string.
        /// </summary>
        /// <param name="type">The type of barcode to generate.</param>
        /// <param name="data">The data to encode in the barcode.</param>
        /// <returns>An SVG string representation of the barcode.</returns>
        /// <exception cref="NotImplementedException">Thrown if the specified barcode type is not implemented.</exception>
        /// <exception cref="ArgumentException">Thrown if the data is invalid for the specified barcode type.</exception>
        public static string Generate(EBarCodeType type, string data)
        {
            var barcode = Encode(type, data);
            var renderer = new Barcoder.Renderer.Svg.SvgRenderer();

            using (var stream = new MemoryStream())
            using (var reader = new StreamReader(stream))
            {
                renderer.Render(barcode, stream);
                stream.Position = 0;

                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Encodes data into a barcode object based on the specified type.
        /// Applies appropriate encoding parameters for each barcode standard.
        /// </summary>
        /// <param name="type">The type of barcode to encode.</param>
        /// <param name="data">The data to encode.</param>
        /// <returns>An <see cref="IBarcode"/> instance containing the encoded barcode data.</returns>
        /// <exception cref="NotImplementedException">Thrown if the specified barcode type is not implemented.</exception>
        /// <exception cref="ArgumentException">Thrown if the data format is incompatible with the barcode type.</exception>
        private static IBarcode Encode(EBarCodeType type, string data)
        {
            switch (type)
            {
                case EBarCodeType.TwoToFive:
                    return TwoToFiveEncoder.Encode(data, interleaved: true, includeChecksum: true);
                case EBarCodeType.Aztec:
                    return AztecEncoder.Encode(data);
                case EBarCodeType.Code39:
                    return Code39Encoder.Encode(data, includeChecksum: true, fullAsciiMode: true);
                case EBarCodeType.Code93:
                    return Code93Encoder.Encode(data, includeChecksum: true, fullAsciiMode: true);
                case EBarCodeType.Code128:
                    return Code128Encoder.Encode(data, includeChecksum: true, gs1ModeEnabled: false);
                case EBarCodeType.Code128GS1:
                    return Code128Encoder.Encode(data, includeChecksum: true, gs1ModeEnabled: true);
                case EBarCodeType.DataMatrixECC200:
                    return DataMatrixEncoder.Encode(data, gs1ModeEnabled: false);
                case EBarCodeType.DataMatrixGS1:
                    return DataMatrixEncoder.Encode(data, gs1ModeEnabled: true);
                case EBarCodeType.EAN:
                    return EanEncoder.Encode(data);
                case EBarCodeType.KIX:
                    return KixEncoder.Encode(data);
                case EBarCodeType.PDF417:
                    return Pdf417Encoder.Encode(data, securityLevel: 0);
                case EBarCodeType.QRCode:
                    return QrEncoder.Encode(data, errorCorrectionLevel: ErrorCorrectionLevel.Q, encoding: Encoding.Unicode);
                case EBarCodeType.RM4SC:
                    return RoyalMailFourStateCodeEncoder.Encode(data);
                case EBarCodeType.UPCA:
                    return UpcAEncoder.Encode(data);
                case EBarCodeType.UPCE:
                    return UpcEEncoder.Encode(data, numberSystem: UpcENumberSystem.One);
                default:
                    throw new NotImplementedException($"The barcode type {type} is not implemented yet.");
            }
        }
    }
}
