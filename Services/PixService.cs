using QRCoder;
using System.Text;

namespace ApiEntregasMentoria.Services
{
    public class PixService
    {
        public string GeneratePixQrCode(decimal amount, string description, string pixKey = "12345678901")
        {
            var pixPayload = GeneratePixPayload(amount, description, pixKey);
            return GenerateQrCodeBase64(pixPayload);
        }

        private string GeneratePixPayload(decimal amount, string description, string pixKey)
        {
            var merchantName = "SoccerBet";
            var merchantCity = "SAO PAULO";
            var txId = Guid.NewGuid().ToString("N")[..25];

            var payload = new StringBuilder();
            payload.Append("000201"); // Payload Format Indicator
            payload.Append("010212"); // Point of Initiation Method
            payload.Append($"26{GetPixKeyField(pixKey)}"); // Merchant Account Information
            payload.Append("52040000"); // Merchant Category Code
            payload.Append("5303986"); // Transaction Currency (BRL)
            payload.Append($"54{amount:00.00}".Replace(".", "").PadLeft(4, '0')); // Transaction Amount
            payload.Append("5802BR"); // Country Code
            payload.Append($"59{merchantName.Length:D2}{merchantName}"); // Merchant Name
            payload.Append($"60{merchantCity.Length:D2}{merchantCity}"); // Merchant City
            payload.Append($"62{GetAdditionalDataField(txId, description)}"); // Additional Data Field

            var crc = CalculateCRC16(payload.ToString() + "6304");
            payload.Append($"6304{crc:X4}"); // CRC16

            return payload.ToString();
        }

        private string GetPixKeyField(string pixKey)
        {
            var field = $"0014br.gov.bcb.pix01{pixKey.Length:D2}{pixKey}";
            return $"{field.Length:D2}{field}";
        }

        private string GetAdditionalDataField(string txId, string description)
        {
            var field = $"05{txId.Length:D2}{txId}";
            if (!string.IsNullOrEmpty(description))
                field += $"02{description.Length:D2}{description}";
            return $"{field.Length:D2}{field}";
        }

        private string GenerateQrCodeBase64(string payload)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.M);
            using var qrCode = new PngByteQRCode(qrCodeData);
            var qrCodeBytes = qrCode.GetGraphic(20);
            return Convert.ToBase64String(qrCodeBytes);
        }

        private ushort CalculateCRC16(string payload)
        {
            var bytes = Encoding.UTF8.GetBytes(payload);
            ushort crc = 0xFFFF;

            foreach (var b in bytes)
            {
                crc ^= (ushort)(b << 8);
                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x8000) != 0)
                        crc = (ushort)((crc << 1) ^ 0x1021);
                    else
                        crc <<= 1;
                }
            }

            return (ushort)(crc & 0xFFFF);
        }
    }
}
