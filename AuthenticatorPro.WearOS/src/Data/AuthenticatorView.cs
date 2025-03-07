// Copyright (C) 2022 jmh
// SPDX-License-Identifier: GPL-3.0-only

using AuthenticatorPro.Droid.Shared.Query;
using AuthenticatorPro.Shared.Data;
using AuthenticatorPro.WearOS.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AuthenticatorPro.WearOS.Data
{
    internal class AuthenticatorView : IReadOnlyList<WearAuthenticator>
    {
        private readonly ListCache<WearAuthenticator> _cache;
        private List<WearAuthenticator> _view;
        private string _categoryId;
        private SortMode _sortMode;

        public string CategoryId
        {
            get => _categoryId;
            set
            {
                _categoryId = value;
                Update();
            }
        }

        public SortMode SortMode
        {
            get => _sortMode;
            set
            {
                _sortMode = value;
                Update();
            }
        }

        public AuthenticatorView(ListCache<WearAuthenticator> cache, string categoryId, SortMode sortMode)
        {
            _cache = cache;
            _categoryId = categoryId;
            _sortMode = sortMode;
            Update();
        }

        public void Update()
        {
            var view = _cache.GetItems().AsEnumerable();

            if (CategoryId != null)
            {
                view = view.Where(a => a.Categories != null && a.Categories.Any(c => c.CategoryId == CategoryId));

                if (SortMode == SortMode.Custom)
                {
                    view = view.OrderBy(a => a.Categories.First(c => c.CategoryId == CategoryId).Ranking);
                }
            }

            view = SortMode switch
            {
                SortMode.AlphabeticalAscending => view.OrderBy(a => a.Issuer).ThenBy(a => a.Username),
                SortMode.AlphabeticalDescending => view.OrderByDescending(a => a.Issuer)
                    .ThenByDescending(a => a.Username),
                SortMode.Custom when CategoryId == null => view.OrderBy(a => a.Ranking).ThenBy(a => a.Issuer)
                    .ThenBy(a => a.Username),
                _ => view
            };

            _view = view.ToList();
        }

        public int FindIndex(Predicate<WearAuthenticator> predicate)
        {
            return _view.FindIndex(predicate);
        }

        public IEnumerator<WearAuthenticator> GetEnumerator()
        {
            return _view.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _view.Count;

        public WearAuthenticator this[int index] => _view[index];
    }
}