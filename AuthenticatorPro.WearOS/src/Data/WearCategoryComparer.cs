// Copyright (C) 2022 jmh
// SPDX-License-Identifier: GPL-3.0-only

using AuthenticatorPro.Droid.Shared.Query;
using System;
using System.Collections.Generic;

namespace AuthenticatorPro.WearOS.Data
{
    internal class WearCategoryComparer : IEqualityComparer<WearCategory>
    {
        public bool Equals(WearCategory x, WearCategory y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null))
            {
                return false;
            }

            if (ReferenceEquals(y, null))
            {
                return false;
            }

            if (x.GetType() != y.GetType())
            {
                return false;
            }

            return x.Id == y.Id && x.Name == y.Name;
        }

        public int GetHashCode(WearCategory obj)
        {
            return HashCode.Combine(obj.Id, obj.Name);
        }
    }
}