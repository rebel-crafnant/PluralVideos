namespace DecryptVideos.Encryption
{
    public class VideoEncryption
    {
        private static bool useV1 = true;
        public static string string1_v2 = "\0¿{U9\x0001®`ë\x0013Ñ[\x001BÏ";
        public static string string2_v2 = "\x0002\x008D\a\x0099\x0089\x009A%\x0084K°súÁ48äcz@\x009F,í>ö 2\vß\n@*í\vz\x008C\x0004\x00BD\x0093\0ÜeË\x0086\x001F\bÖ\x009E ADÓg&ì¶\x0017\x008DÀ\x0014{µìß\x0088Ø\x009FòÕÄ\x0081pªªtC\x008A@\x009C2:Åf\\\\\x00ADè\x009Eý\x0002g\x0003|ØBf\x0092 ";

        public static void XorBuffer(byte[] buff, int length, long position)
        {
            string str1 = "pluralsight";
            string str2 = "\x0006?zY¢\x00B2\x0085\x009FL\x00BEî0Ö.ì\x0017#©>Å£Q\x0005¤°\x00018Þ^\x008Eú\x0019Lqß'\x009D\x0003ßE\x009EM\x0080'x:\0~\x00B9\x0001ÿ 4\x00B3õ\x0003Ã§Ê\x000EAË\x00BC\x0090è\x009Eî~\x008B\x009Aâ\x001B¸UD<\x007FKç*\x001Döæ7H\v\x0015Arý*v÷%Âþ\x00BEä;pü";
            for (int index = 0; index < length; ++index)
            {
                byte num = (byte)((ulong)((int)str1[(int)((position + (long)index) % (long)str1.Length)] ^ (int)str2[(int)((position + (long)index) % (long)str2.Length)]) ^ (ulong)((position + (long)index) % 251L));
                buff[index] = (byte)((uint)buff[index] ^ (uint)num);
            }
        }

        public static void XorBufferV2(byte[] buff, int length, long position)
        {
            for (int index = 0; index < length; ++index)
            {
                byte num = (byte)((ulong)((int)string1_v2[(int)((position + (long)index) % (long)string1_v2.Length)] ^ (int)string2_v2[(int)((position + (long)index) % (long)string2_v2.Length)]) ^ (ulong)((position + (long)index) % 251L));
                buff[index] = (byte)((uint)buff[index] ^ (uint)num);
            }
        }

        public static void EncryptBuffer(byte[] buff, int length, long position)
        {
            XorBufferV2(buff, length, position);
        }

        public static void DecryptBuffer(byte[] buff, int length, long position)
        {
            if (position == 0L && length > 3)
            {
                XorBuffer(buff, length, position);
                if (buff[0] == (byte)0 && buff[1] == (byte)0 && buff[2] == (byte)0)
                {
                    useV1 = true;
                }
                else
                {
                    XorBuffer(buff, length, position);
                    XorBufferV2(buff, length, position);
                    if (buff[0] == (byte)0 && buff[1] == (byte)0)
                    {
                        int num = (int)buff[2];
                    }
                    useV1 = false;
                }
            }
            else if (useV1)
                XorBuffer(buff, length, position);
            else
                XorBufferV2(buff, length, position);
        }
    }
}
