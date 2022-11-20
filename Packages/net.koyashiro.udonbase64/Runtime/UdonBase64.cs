namespace Koyashiro.UdonBase64
{
    using Koyashiro.UdonException;

    public static class UdonBase64
    {
        public static byte[] Decode(string input, bool urlSafe = false)
        {
            if (input == null)
            {
                UdonException.ThrowArgumentNullException(nameof(input));
                return default;
            }

            if (input.Length == 0)
            {
                return new byte[0];
            }

            var buf = new byte[input.Length + 3];
            var count = 0;
            for (var i = 0; i < input.Length; i++)
            {
                if (input[i] == '=' || input[i] == '\n' || input[i] == '\r')
                {
                    for (var j = i + 1; j < input.Length; j++)
                    {
                        if (input[j] != '=' && input[j] != '\n' && input[j] != '\r')
                        {
                            UdonException.ThrowException("Invalid input.");
                            return default;
                        }
                    }
                    break;
                }

                if (!TryDecodeChar(input[i], urlSafe, out var b))
                {
                    UdonException.ThrowException($"Invalid input. '{input[i]}', pos: {i}");
                }
                buf[count++] = b;
            }

            int offset;
            switch (count % 4)
            {
                case 0:
                    offset = 0;
                    break;
                case 1:
                    buf[count++] = (byte)0x00;
                    buf[count++] = (byte)0x00;
                    buf[count++] = (byte)0x00;
                    offset = 2;
                    break;
                case 2:
                    buf[count++] = (byte)0x00;
                    buf[count++] = (byte)0x00;
                    offset = 2;
                    break;
                case 3:
                    buf[count++] = (byte)0x00;
                    offset = 1;
                    break;
                default:
                    offset = default;
                    break;
            }

            var output = new byte[count / 4 * 3 - offset];
            for (var i = 0; i < count / 4; i++)
            {
                var a = buf[i * 4];
                var b = buf[i * 4 + 1];
                var c = buf[i * 4 + 2];
                var d = buf[i * 4 + 3];

                var x = (byte)(a << 2 | (b & 0b00110000) >> 4);
                var y = (byte)((b & 0b00001111) << 4 | (c & 0b00111100) >> 2);
                var z = (byte)((c & 0b00000011) << 6 | d);

                if (i != count / 4 - 1)
                {
                    output[i * 3] = x;
                    output[i * 3 + 1] = y;
                    output[i * 3 + 2] = z;
                }
                // Last
                else
                {
                    switch (offset)
                    {
                        case 2:
                            output[i * 3] = x;
                            break;
                        case 1:
                            output[i * 3] = x;
                            output[i * 3 + 1] = y;
                            break;
                        case 0:
                            output[i * 3] = x;
                            output[i * 3 + 1] = y;
                            output[i * 3 + 2] = z;
                            break;
                    }
                }
            }

            return output;
        }

        private static bool TryDecodeChar(char c, bool urlSafe, out byte b)
        {
            // 000000 ~ 011001
            if ('A' <= c && c <= 'Z')
            {
                b = (byte)(c - 'A');
                return true;
            }

            // 011010 ~ 110011
            if ('a' <= c && c <= 'z')
            {
                b = (byte)(0b011010 + c - 'a');
                return true;
            }

            // 110100 ~ 111101
            if ('0' <= c && c <= '9')
            {
                b = (byte)(0b110100 + c - '0');
                return true;
            }

            if (urlSafe)
            {
                // 111110
                if (c == '-')
                {
                    b = 0b111110;
                    return true;
                }

                // 111111
                if (c == '_')
                {
                    b = 0b111111;
                    return true;
                }
            }
            else
            {
                // 111110
                if (c == '+')
                {
                    b = 0b111110;
                    return true;
                }

                // 111111
                if (c == '/')
                {
                    b = 0b111111;
                    return true;
                }
            }


            b = default;
            return false;
        }
    }
}
