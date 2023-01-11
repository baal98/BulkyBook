using System.Security.Claims;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private shoppingCartVM shoppingCartVM;

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartVM = new shoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product"),
                OrderHeader = new()
            };
            foreach (var cart in shoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50,
                    cart.Product.Price100);
                shoppingCartVM.CartTotal += (cart.Price * cart.Count);
            }
            return View(shoppingCartVM);
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartVM = new shoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product"),
                OrderHeader = new()
            };
            foreach (var cart in shoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50,
                    cart.Product.Price100);
                shoppingCartVM.CartTotal += (cart.Price * cart.Count);
            }
            shoppingCartVM.OrderHeader.OrderTotal = shoppingCartVM.CartTotal;
            shoppingCartVM.OrderHeader.Name = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value).Name;
            shoppingCartVM.OrderHeader.PhoneNumber = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value).PhoneNumber;
            shoppingCartVM.OrderHeader.StreetAddress = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value).StreetAddress;
            shoppingCartVM.OrderHeader.City = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value).City;
            shoppingCartVM.OrderHeader.State = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value).State;
            shoppingCartVM.OrderHeader.PostalCode = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value).PostalCode;
            return View(shoppingCartVM);
        }
        
        public IActionResult Minus(int cardId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cardId);
            if (cart.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cart);
            }
            else
            {
                _unitOfWork.ShoppingCart.DecrementCount(cart, 1);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Plus(int cardId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cardId);
            _unitOfWork.ShoppingCart.IncrementCount(cart, 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cardId)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cardId);
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public int OrderTotal { get; set; }

        private double GetPriceBasedOnQuantity(double quantity, double price, double price50, double price100)
        {
            if (quantity <= 50)
            {
                return price;
            }
            else
            {
                if (quantity <= 100)
                {
                    return price50;
                }

                return price100;
            }
        }
    }
}
