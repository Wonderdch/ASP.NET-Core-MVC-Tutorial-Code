using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Heavy.Web.Data;
using Heavy.Web.Models;
using Heavy.Web.Services;
using Heavy.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Heavy.Web.Controllers
{
    [Authorize(Policy = "编辑专辑2")]
    public class AlbumController : Controller
    {
        private readonly ILogger<AlbumController> _logger;

        private readonly IMemoryCache _memoryCache;

        private readonly IDistributedCache _distributedCache;

        private readonly IAlbumService _albumService;

        private readonly HtmlEncoder _htmlEncoder;

        public AlbumController(
            IAlbumService albumService,
            HtmlEncoder htmlEncoder,
            ILogger<AlbumController> logger,
            IMemoryCache memoryCache,
            IDistributedCache distributedCache)
        {
            _albumService = albumService;
            _htmlEncoder = htmlEncoder;
            _logger = logger;
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }

        // GET: Album
        public async Task<ActionResult> Index()
        {
            List<Album> cachedAlbums;

            var cachedAlbumsString = _distributedCache.Get(CacheEntryConstants.AlbumsOfToday);
            if (cachedAlbumsString == null)
            {
                cachedAlbums = await _albumService.GetAllAsync();
                var serializedString = JsonConvert.SerializeObject(cachedAlbums);
                byte[] encodedAlbums = Encoding.UTF8.GetBytes(serializedString);


                var cacheEntryOptions = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(30));

                _distributedCache.Set(CacheEntryConstants.AlbumsOfToday, encodedAlbums, cacheEntryOptions);
            }
            else
            {
                byte[] encodedAlbums = _distributedCache.Get(CacheEntryConstants.AlbumsOfToday);
                var serializedString = Encoding.UTF8.GetString(encodedAlbums);
                cachedAlbums = JsonConvert.DeserializeObject<List<Album>>(serializedString);
            }

            return View(cachedAlbums);

            //if (!_memoryCache.TryGetValue(
            //    CacheEntryConstants.AlbumsOfToday,
            //    out List<Album> cachedAlbums))
            //{
            //    cachedAlbums = await _albumService.GetAllAsync();

            //    var cacheEntryOptions = new MemoryCacheEntryOptions()
            //        .SetSlidingExpiration(TimeSpan.FromSeconds(30));

            //    cacheEntryOptions.RegisterPostEvictionCallback(FillCache, this);

            //    _memoryCache.Set(CacheEntryConstants.AlbumsOfToday, cachedAlbums, cacheEntryOptions);
            //}

            //return View(cachedAlbums);
        }

        private void FillCache(object key, object value, EvictionReason reason, object state)
        {
            // 不具体实现
            Console.WriteLine("Cache entry is evicted!");
        }

        // GET: Album/Details/5
        public async Task<ActionResult> Details(int id)
        {
            // 推荐，会记录单独的 id 参数
            _logger.LogInformation(MyLogEventIds.AlbumPage, "Visiting Album {0}", id);

            // 不推荐，仅记录了字符串
            _logger.LogInformation(MyLogEventIds.AlbumPage, $"Visiting Album {id}");

            var album = await _albumService.GetByIdAsync(id);
            if (album == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(album);
        }

        // GET: Album/Create
        public ActionResult Create()
        {
            var newModel = new AlbumCreateViewModel();
            return View(newModel);
        }

        // POST: Album/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AlbumCreateViewModel albumCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Model is not valid");
                return View();

            }
            try
            {
                var newModel = await _albumService.AddAsync(new Album
                {
                    Artist = _htmlEncoder.Encode(albumCreateViewModel.Artist),
                    Title = albumCreateViewModel.Title,
                    CoverUrl = albumCreateViewModel.CoverUrl,
                    Price = albumCreateViewModel.Price,
                    ReleaseDate = albumCreateViewModel.ReleaseDate
                });
                return RedirectToAction(nameof(Details), new { id = newModel.Id });
            }
            catch
            {
                return View(albumCreateViewModel);
            }
        }

        // GET: Album/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var model = await _albumService.GetByIdAsync(id);
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var editModel = new AlbumUpdateViewModel
            {
                Artist = model.Artist,
                Title = model.Title,
                CoverUrl = model.CoverUrl,
                Price = model.Price,
                ReleaseDate = model.ReleaseDate
            };
            return View(editModel);
        }

        // POST: Album/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, AlbumUpdateViewModel albumUpdateViewModel)
        {
            var model = await _albumService.GetByIdAsync(id);
            if (model == null)
            {
                // return NotFound();
                return View(albumUpdateViewModel);
            }

            try
            {
                model.Artist = albumUpdateViewModel.Artist;
                model.Title = albumUpdateViewModel.Title;
                model.CoverUrl = albumUpdateViewModel.CoverUrl;
                model.ReleaseDate = albumUpdateViewModel.ReleaseDate;
                model.Price = albumUpdateViewModel.Price;

                await _albumService.UpdateAsync(model);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(albumUpdateViewModel);
            }
        }

        // GET: Album/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _albumService.GetByIdAsync(id);
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // POST: Album/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            var model = await _albumService.GetByIdAsync(id);
            if (model == null)
            {
                return RedirectToAction(nameof(Index));
                // return NotFound();
            }

            try
            {
                await _albumService.DeleteAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}