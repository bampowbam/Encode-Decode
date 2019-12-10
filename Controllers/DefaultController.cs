using System.Web.Mvc;

namespace WebApplication1.Controllers
{
  public class DefaultController : Controller
  {

    private int EncodeValue(int value)
    {
      value += 8192;
      return (value & 0x007F) + ((value & 0x3F80) << 1);
    }

    private int DecodeValue(byte high, byte low)
    {
      return (((high & 0x7F) << 7) + (low & 0x7F)) - 8192;
    }

    [HttpPost]
    public ActionResult Encode(int value)
    {
      if (value >= -8192 && value < 8191)
        return View((object)EncodeValue(value).ToString("X4").ToLower());
      else
        return View((object)null);
    }

    [HttpGet]
    public ActionResult Encode()
    {
      return View();
    }

    private int? IntFromHex(string value)
    {
      string hex = "0123456789ABCDEF";
      value = value.ToUpper();
      int highIndex = hex.IndexOf(value[0]);
      int lowIndex = hex.IndexOf(value[1]);
      if (highIndex >= 0 && lowIndex >= 0)
        return highIndex * 16 + lowIndex;
      else
        return null;
    }

    [HttpPost]
    public ActionResult Decode(string highValue, string lowValue)
    {
      if (!string.IsNullOrEmpty(highValue) && !string.IsNullOrEmpty(lowValue) && highValue.Length == 2 && lowValue.Length == 2)
      {
        int? highValueAsInt = IntFromHex(highValue.ToUpper());
        int? lowValueAsInt = IntFromHex(lowValue.ToUpper());
        if (highValueAsInt.HasValue && lowValueAsInt.HasValue)
        {
          return View((object)DecodeValue((byte)highValueAsInt.Value, (byte)lowValueAsInt.Value));
        }
        else
          return View((object)null);
      }
      else
        return View((object)null);
    }

    [HttpGet]
    public ActionResult Decode()
    {
      return View();
    }
  }
}