﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurant.API.ShoppingCartAPI.Database;
using Restaurant.API.ShoppingCartAPI.Models;
using Restaurant.API.ShoppingCartAPI.Models.Dto;
using Restaurant.API.ShoppingCartAPI.Repository.Interfaces;

namespace Restaurant.API.ShoppingCartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private IMapper _mapper;

        public CartRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> ClearCart()
        {
            var cartHeaderFromDb = await _dbContext.CartHeaders.OrderByDescending(x => x.CartHeaderId).FirstOrDefaultAsync();

            if (cartHeaderFromDb != null)
            {
                _dbContext.CartDetails.RemoveRange(_dbContext.CartDetails.Where(u => u.CartHeaderId == cartHeaderFromDb.CartHeaderId));
                _dbContext.CartHeaders.Remove(cartHeaderFromDb);
                await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<CartDto> CreateUpdateCart(CartDto cartDto)
        {
            Cart cart = _mapper.Map<Cart>(cartDto);

            var prodInDb = await _dbContext.Products
                .FirstOrDefaultAsync(u => u.ProductId == cartDto.CartDetails.FirstOrDefault()
                .ProductId);
            if (prodInDb == null)
            {
                _dbContext.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await _dbContext.SaveChangesAsync();
            }

            var cartHeaderFromDb = await _dbContext.CartHeaders.AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == cart.Header.UserId);

            if (cartHeaderFromDb == null)
            {
                _dbContext.CartHeaders.Add(cart.Header);
                await _dbContext.SaveChangesAsync();
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.Header.CartHeaderId;
                cart.CartDetails.FirstOrDefault().Product = null;
                _dbContext.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                var cartDetailsFromDb = await _dbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                    u => u.ProductId == cart.CartDetails.FirstOrDefault().ProductId &&
                    u.CartHeaderId == cartHeaderFromDb.CartHeaderId);

                if (cartDetailsFromDb == null)
                {
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    _dbContext.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Count += cartDetailsFromDb.Count;
                    cart.CartDetails.FirstOrDefault().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                    _dbContext.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await _dbContext.SaveChangesAsync();
                }
            }

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> GetCart()
        {
            Cart cart = new()
            {
                Header = await _dbContext.CartHeaders.OrderByDescending(x => x.CartHeaderId).FirstOrDefaultAsync()
            };

            cart.CartDetails = _dbContext.CartDetails.Where(u => u.CartHeaderId == cart.Header.CartHeaderId).Include(u => u.Product);

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<bool> RemoveFromCart(int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = await _dbContext.CartDetails
                    .FirstOrDefaultAsync(u => u.CartDetailsId == cartDetailsId);

                int totalCountOfCartItems = _dbContext.CartDetails
                    .Where(u => u.CartHeaderId == cartDetails.CartHeaderId).Count();

                _dbContext.CartDetails.Remove(cartDetails);

                if (totalCountOfCartItems == 1)
                {
                    var cartHeaderToRemove = await _dbContext.CartHeaders
                        .FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);

                    _dbContext.CartHeaders.Remove(cartHeaderToRemove);
                }

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
