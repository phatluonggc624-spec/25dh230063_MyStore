using System.Web.Mvc;

namespace _25dh230063_MyStore.Areas.Admin.Filters
{
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Kiểm tra xem có Session Admin chưa?
            var sessionAdmin = filterContext.HttpContext.Session["AdminUser"];

            if (sessionAdmin == null)
            {
                // Nếu chưa đăng nhập -> Chuyển hướng về trang Login
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new { controller = "Account", action = "Login", area = "Admin" }
                    )
                );
            }
            base.OnActionExecuting(filterContext);
        }
    }
}