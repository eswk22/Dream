using System;
 namespace ExecutionEngine.Common.Connect
 {
		 using Key = com.jagacy.Key;
		 using Session3270 = com.jagacy.Session3270;
		 using JagacyException = com.jagacy.util.JagacyException;
		 using SessionObjectInterface = com.resolve.rsbase.SessionObjectInterface;
		 using Log = com.resolve.util.Log;
		 using Application.Utility.Logging;
		 public class TN3270Connect : Session3270, SessionObjectInterface
	 {	
//ORIGINAL LINE: public TN3270Connect(String sessionName, String host, int port) throws com.jagacy.util.JagacyException
	   public TN3270Connect(string sessionName, string host, int port) : base(sessionName, host, port)
	   {	/*  26 */	     
	/*  28 */		 open();
	   }	   
//ORIGINAL LINE: public TN3270Connect(String sessionName, String host, int port, String terminalType) throws com.jagacy.util.JagacyException
	   public TN3270Connect(string sessionName, string host, int port, string terminalType) : base(sessionName, host, port, terminalType)
	   {	/*  33 */	     
	/*  35 */		 open();
	   }	   
//ORIGINAL LINE: public TN3270Connect(String sessionName, String host, int port, String terminalType, String sslKeyFile, String sslKeyPassword) throws com.jagacy.util.JagacyException
	   public TN3270Connect(string sessionName, string host, int port, string terminalType, string sslKeyFile, string sslKeyPassword) : base(sessionName, host, port, terminalType)
	   {	/*  40 */	     
	/*  42 */		 open(sslKeyFile, sslKeyPassword);
	   }	   
		   public virtual void close()
	   {			 try
		 {	/*  49 */		   base.close();
		 }			 catch (Exception e)
		 {	/*  53 */		   _logger.Warn("Error closing connection: " + e.Message);
		 }	
	   }	   
		   ~TN3270Connect()
	   {			 try
		 {	/*  61 */		   base.close();
		 }			 catch (Exception e)
		 {	/*  65 */		   _logger.Warn("Error closing connection: " + e.Message);
		 }	     
	/*  68 */
//JAVA TO C# CONVERTER NOTE: The base class finalizer method is automatically called in C#://		 base.finalize();
	   }	   
	/*  72 */	   public static Key ATTN = Key.ATTN;
	/*  73 */	   public static int ATTN_ID = Key.ATTN_ID;
	/*  74 */	   public static string ATTN_NAME = "ATTN";
	/*  75 */	   public static Key ATTN_WAIT = Key.ATTN_WAIT;
	/*  76 */	   public static Key BACK_TAB = Key.BACK_TAB;
	/*  77 */	   public static int BACK_TAB_ID = Key.BACK_TAB_ID;
	/*  78 */	   public static string BACK_TAB_NAME = "BACK TAB";
	/*  79 */	   public static Key BACKSPACE = Key.BACKSPACE;
	/*  80 */	   public static int BACKSPACE_ID = Key.BACKSPACE_ID;
	/*  81 */	   public static string BACKSPACE_NAME = "BACKSPACE";
		   public static Key CLEAR = Key.CLEAR;
		   public static int CLEAR_ID = Key.CLEAR_ID;
		   public static string CLEAR_NAME = "CLEAR";
		   public static Key CLEAR_WAIT = Key.CLEAR_WAIT;
		   public static Key CURSOR_SELECT = Key.CURSOR_SELECT;
		   public static int CURSOR_SELECT_ID = Key.CURSOR_SELECT_ID;
		   public static string CURSOR_SELECT_NAME = "CURSOR SELECT";
	/*  89 */	   public static Key CURSOR_SELECT_WAIT = Key.CURSOR_SELECT_WAIT;
	/*  90 */	   public static Key DELETE = Key.DELETE;
	/*  91 */	   public static int DELETE_ID = Key.DELETE_ID;
	/*  92 */	   public static string DELETE_NAME = "DELETE";
	/*  93 */	   public static Key DOWN_ARROW = Key.DOWN_ARROW;
	/*  94 */	   public static int DOWN_ARROW_ID = Key.DOWN_ARROW_ID;
	/*  95 */	   public static string DOWN_ARROW_NAME = "DOWN ARROW";
	/*  96 */	   public static Key DUPLICATE = Key.DUPLICATE;
	/*  97 */	   public static int DUPLICATE_ID = Key.DUPLICATE_ID;
	/*  98 */	   public static string DUPLICATE_NAME = "DUPLICATE";
	/*  99 */	   public static Key ENTER = Key.ENTER;
	/* 100 */	   public static int ENTER_ID = Key.ENTER_ID;
	/* 101 */	   public static string ENTER_NAME = "ENTER";
	/* 102 */	   public static Key ENTER_WAIT = Key.ENTER_WAIT;
	/* 103 */	   public static Key ERASE_EOF = Key.ERASE_EOF;
	/* 104 */	   public static int ERASE_EOF_ID = Key.ERASE_EOF_ID;
	/* 105 */	   public static string ERASE_EOF_NAME = "ERASE EOF";
	/* 106 */	   public static Key ERASE_INPUT = Key.ERASE_INPUT;
	/* 107 */	   public static int ERASE_INPUT_ID = Key.ERASE_INPUT_ID;
	/* 108 */	   public static string ERASE_INPUT_NAME = "ERASE INPUT";
	/* 109 */	   public static Key HOME = Key.HOME;
	/* 110 */	   public static int HOME_ID = Key.HOME_ID;
	/* 111 */	   public static string HOME_NAME = "HOME";
	/* 112 */	   public static Key INSERT = Key.INSERT;
	/* 113 */	   public static int INSERT_ID = Key.INSERT_ID;
	/* 114 */	   public static string INSERT_NAME = "INSERT";
	/* 115 */	   public static Key KEYPAD_0 = Key.KEYPAD_0;
	/* 116 */	   public static int KEYPAD_0_ID = Key.KEYPAD_0_ID;
	/* 117 */	   public static string KEYPAD_0_NAME = "KEYPAD 0";
	/* 118 */	   public static Key KEYPAD_1 = Key.KEYPAD_1;
	/* 119 */	   public static int KEYPAD_1_ID = Key.KEYPAD_1_ID;
	/* 120 */	   public static string KEYPAD_1_NAME = "KEYPAD 1";
	/* 121 */	   public static Key KEYPAD_2 = Key.KEYPAD_2;
	/* 122 */	   public static int KEYPAD_2_ID = Key.KEYPAD_2_ID;
	/* 123 */	   public static string KEYPAD_2_NAME = "KEYPAD 2";
	/* 124 */	   public static Key KEYPAD_3 = Key.KEYPAD_3;
	/* 125 */	   public static int KEYPAD_3_ID = Key.KEYPAD_3_ID;
	/* 126 */	   public static string KEYPAD_3_NAME = "KEYPAD 3";
	/* 127 */	   public static Key KEYPAD_4 = Key.KEYPAD_4;
	/* 128 */	   public static int KEYPAD_4_ID = Key.KEYPAD_4_ID;
	/* 129 */	   public static string KEYPAD_4_NAME = "KEYPAD 4";
	/* 130 */	   public static Key KEYPAD_5 = Key.KEYPAD_5;
	/* 131 */	   public static int KEYPAD_5_ID = Key.KEYPAD_5_ID;
	/* 132 */	   public static string KEYPAD_5_NAME = "KEYPAD 5";
	/* 133 */	   public static Key KEYPAD_6 = Key.KEYPAD_6;
	/* 134 */	   public static int KEYPAD_6_ID = Key.KEYPAD_6_ID;
	/* 135 */	   public static string KEYPAD_6_NAME = "KEYPAD 6";
	/* 136 */	   public static Key KEYPAD_7 = Key.KEYPAD_7;
	/* 137 */	   public static int KEYPAD_7_ID = Key.KEYPAD_7_ID;
	/* 138 */	   public static string KEYPAD_7_NAME = "KEYPAD 7";
	/* 139 */	   public static Key KEYPAD_8 = Key.KEYPAD_8;
	/* 140 */	   public static int KEYPAD_8_ID = Key.KEYPAD_8_ID;
	/* 141 */	   public static string KEYPAD_8_NAME = "KEYPAD 8";
	/* 142 */	   public static Key KEYPAD_9 = Key.KEYPAD_9;
	/* 143 */	   public static int KEYPAD_9_ID = Key.KEYPAD_9_ID;
	/* 144 */	   public static string KEYPAD_9_NAME = "KEYPAD 9";
	/* 145 */	   public static Key KEYPAD_COMMA = Key.KEYPAD_COMMA;
	/* 146 */	   public static int KEYPAD_COMMA_ID = Key.KEYPAD_COMMA_ID;
	/* 147 */	   public static string KEYPAD_COMMA_NAME = "KEYPAD COMMA";
	/* 148 */	   public static Key KEYPAD_HYPHEN = Key.KEYPAD_HYPHEN;
	/* 149 */	   public static int KEYPAD_HYPHEN_ID = Key.KEYPAD_HYPHEN_ID;
	/* 150 */	   public static string KEYPAD_HYPHEN_NAME = "KEYPAD HYPHEN";
	/* 151 */	   public static Key KEYPAD_PERIOD = Key.KEYPAD_PERIOD;
	/* 152 */	   public static int KEYPAD_PERIOD_ID = Key.KEYPAD_PERIOD_ID;
	/* 153 */	   public static string KEYPAD_PERIOD_NAME = "KEYPAD PERIOD";
	/* 154 */	   public static Key KEYPAD_RETURN = Key.KEYPAD_RETURN;
	/* 155 */	   public static int KEYPAD_RETURN_ID = Key.KEYPAD_RETURN_ID;
	/* 156 */	   public static string KEYPAD_RETURN_NAME = "KEYPAD RETURN";
	/* 157 */	   public static Key LEFT_ARROW = Key.LEFT_ARROW;
	/* 158 */	   public static int LEFT_ARROW_ID = Key.LEFT_ARROW_ID;
	/* 159 */	   public static string LEFT_ARROW_NAME = "LEFT ARROW";
	/* 160 */	   public static int MAX_ID = Key.MAX_ID;
	/* 161 */	   public static Key NEWLINE = Key.NEWLINE;
	/* 162 */	   public static int NEWLINE_ID = Key.NEWLINE_ID;
	/* 163 */	   public static string NEWLINE_NAME = "NEWLINE";
	/* 164 */	   public static Key PA1 = Key.PA1;
	/* 165 */	   public static int PA1_ID = Key.PA1_ID;
	/* 166 */	   public static string PA1_NAME = "PA1";
	/* 167 */	   public static Key PA1_WAIT = Key.PA1_WAIT;
	/* 168 */	   public static Key PA2 = Key.PA2;
	/* 169 */	   public static int PA2_ID = Key.PA2_ID;
	/* 170 */	   public static string PA2_NAME = "PA2";
	/* 171 */	   public static Key PA2_WAIT = Key.PA2_WAIT;
	/* 172 */	   public static Key PA3 = Key.PA3;
	/* 173 */	   public static int PA3_ID = Key.PA3_ID;
	/* 174 */	   public static string PA3_NAME = "PA3";
	/* 175 */	   public static Key PA3_WAIT = Key.PA3_WAIT;
	/* 176 */	   public static Key PF1 = Key.PF1;
	/* 177 */	   public static int PF1_ID = Key.PF1_ID;
	/* 178 */	   public static string PF1_NAME = "PF1";
	/* 179 */	   public static Key PF1_WAIT = Key.PF1_WAIT;
	/* 180 */	   public static Key PF10 = Key.PF10;
	/* 181 */	   public static int PF10_ID = Key.PF10_ID;
	/* 182 */	   public static string PF10_NAME = "PF10";
	/* 183 */	   public static Key PF10_WAIT = Key.PF10_WAIT;
	/* 184 */	   public static Key PF11 = Key.PF11;
	/* 185 */	   public static int PF11_ID = Key.PF11_ID;
	/* 186 */	   public static string PF11_NAME = "PF11";
	/* 187 */	   public static Key PF11_WAIT = Key.PF11_WAIT;
	/* 188 */	   public static Key PF12 = Key.PF12;
	/* 189 */	   public static int PF12_ID = Key.PF12_ID;
	/* 190 */	   public static string PF12_NAME = "PF12";
	/* 191 */	   public static Key PF12_WAIT = Key.PF12_WAIT;
	/* 192 */	   public static Key PF13 = Key.PF13;
	/* 193 */	   public static int PF13_ID = Key.PF13_ID;
	/* 194 */	   public static string PF13_NAME = "PF13";
	/* 195 */	   public static Key PF13_WAIT = Key.PF13_WAIT;
	/* 196 */	   public static Key PF14 = Key.PF14;
	/* 197 */	   public static int PF14_ID = Key.PF14_ID;
	/* 198 */	   public static string PF14_NAME = "PF14";
	/* 199 */	   public static Key PF14_WAIT = Key.PF14_WAIT;
	/* 200 */	   public static Key PF15 = Key.PF15;
	/* 201 */	   public static int PF15_ID = Key.PF15_ID;
	/* 202 */	   public static string PF15_NAME = "PF15";
	/* 203 */	   public static Key PF15_WAIT = Key.PF15_WAIT;
	/* 204 */	   public static Key PF16 = Key.PF16;
	/* 205 */	   public static int PF16_ID = Key.PF16_ID;
	/* 206 */	   public static string PF16_NAME = "PF16";
	/* 207 */	   public static Key PF16_WAIT = Key.PF16_WAIT;
	/* 208 */	   public static Key PF17 = Key.PF17;
	/* 209 */	   public static int PF17_ID = Key.PF17_ID;
	/* 210 */	   public static string PF17_NAME = "PF17";
	/* 211 */	   public static Key PF17_WAIT = Key.PF17_WAIT;
	/* 212 */	   public static Key PF18 = Key.PF18;
	/* 213 */	   public static int PF18_ID = Key.PF18_ID;
	/* 214 */	   public static string PF18_NAME = "PF18";
	/* 215 */	   public static Key PF18_WAIT = Key.PF18_WAIT;
	/* 216 */	   public static Key PF19 = Key.PF19;
	/* 217 */	   public static int PF19_ID = Key.PF19_ID;
	/* 218 */	   public static string PF19_NAME = "PF19";
	/* 219 */	   public static Key PF19_WAIT = Key.PF19_WAIT;
	/* 220 */	   public static Key PF2 = Key.PF2;
	/* 221 */	   public static int PF2_ID = Key.PF2_ID;
	/* 222 */	   public static string PF2_NAME = "PF2";
	/* 223 */	   public static Key PF2_WAIT = Key.PF2_WAIT;
	/* 224 */	   public static Key PF20 = Key.PF20;
	/* 225 */	   public static int PF20_ID = Key.PF20_ID;
	/* 226 */	   public static string PF20_NAME = "PF20";
	/* 227 */	   public static Key PF20_WAIT = Key.PF20_WAIT;
	/* 228 */	   public static Key PF21 = Key.PF21;
	/* 229 */	   public static int PF21_ID = Key.PF21_ID;
	/* 230 */	   public static string PF21_NAME = "PF21";
	/* 231 */	   public static Key PF21_WAIT = Key.PF21_WAIT;
	/* 232 */	   public static Key PF22 = Key.PF22;
	/* 233 */	   public static int PF22_ID = Key.PF22_ID;
	/* 234 */	   public static string PF22_NAME = "PF22";
	/* 235 */	   public static Key PF22_WAIT = Key.PF22_WAIT;
	/* 236 */	   public static Key PF23 = Key.PF23;
	/* 237 */	   public static int PF23_ID = Key.PF23_ID;
	/* 238 */	   public static string PF23_NAME = "PF23";
	/* 239 */	   public static Key PF23_WAIT = Key.PF23_WAIT;
	/* 240 */	   public static Key PF24 = Key.PF24;
	/* 241 */	   public static int PF24_ID = Key.PF24_ID;
	/* 242 */	   public static string PF24_NAME = "PF24";
	/* 243 */	   public static Key PF24_WAIT = Key.PF24_WAIT;
	/* 244 */	   public static Key PF3 = Key.PF3;
	/* 245 */	   public static int PF3_ID = Key.PF3_ID;
	/* 246 */	   public static string PF3_NAME = "PF3";
	/* 247 */	   public static Key PF3_WAIT = Key.PF3_WAIT;
	/* 248 */	   public static Key PF4 = Key.PF4;
	/* 249 */	   public static int PF4_ID = Key.PF4_ID;
	/* 250 */	   public static string PF4_NAME = "PF4";
	/* 251 */	   public static Key PF4_WAIT = Key.PF4_WAIT;
	/* 252 */	   public static Key PF5 = Key.PF5;
	/* 253 */	   public static int PF5_ID = Key.PF5_ID;
	/* 254 */	   public static string PF5_NAME = "PF5";
	/* 255 */	   public static Key PF5_WAIT = Key.PF5_WAIT;
	/* 256 */	   public static Key PF6 = Key.PF6;
	/* 257 */	   public static int PF6_ID = Key.PF6_ID;
	/* 258 */	   public static string PF6_NAME = "PF6";
	/* 259 */	   public static Key PF6_WAIT = Key.PF6_WAIT;
	/* 260 */	   public static Key PF7 = Key.PF7;
	/* 261 */	   public static int PF7_ID = Key.PF7_ID;
	/* 262 */	   public static string PF7_NAME = "PF7";
	/* 263 */	   public static Key PF7_WAIT = Key.PF7_WAIT;
	/* 264 */	   public static Key PF8 = Key.PF8;
	/* 265 */	   public static int PF8_ID = Key.PF8_ID;
	/* 266 */	   public static string PF8_NAME = "PF8";
	/* 267 */	   public static Key PF8_WAIT = Key.PF8_WAIT;
	/* 268 */	   public static Key PF9 = Key.PF9;
	/* 269 */	   public static int PF9_ID = Key.PF9_ID;
	/* 270 */	   public static string PF9_NAME = "PF9";
	/* 271 */	   public static Key PF9_WAIT = Key.PF9_WAIT;
	/* 272 */	   public static Key RESET = Key.RESET;
	/* 273 */	   public static int RESET_ID = Key.RESET_ID;
	/* 274 */	   public static string RESET_NAME = "RESET";
	/* 275 */	   public static Key RIGHT_ARROW = Key.RIGHT_ARROW;
	/* 276 */	   public static int RIGHT_ARROW_ID = Key.RIGHT_ARROW_ID;
	/* 277 */	   public static string RIGHT_ARROW_NAME = "RIGHT ARROW";
	/* 278 */	   public static Key SYSREQ = Key.SYSREQ;
	/* 279 */	   public static int SYSREQ_ID = Key.SYSREQ_ID;
	/* 280 */	   public static string SYSREQ_NAME = "SYSREQ";
	/* 281 */	   public static Key SYSREQ_WAIT = Key.SYSREQ_WAIT;
	/* 282 */	   public static Key TAB = Key.TAB;
	/* 283 */	   public static int TAB_ID = Key.TAB_ID;
	/* 284 */	   public static string TAB_NAME = "TAB";
	/* 285 */	   public static Key UP_ARROW = Key.UP_ARROW;
	/* 286 */	   public static int UP_ARROW_ID = Key.UP_ARROW_ID;
	/* 287 */	   public static string UP_ARROW_NAME = "UP ARROW";
	 }
	/* Location:              C:\EGI\Projects\iOSS\Architecture Team\Projects\Resolve\Libs\resolve-remote.jar!\com\resolve\connect\TN3270Connect.class
	 * Java compiler version: 7 (51.0)
	 * JD-Core Version:       0.7.1
	 */
 }