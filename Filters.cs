namespace NavitaireTE
{
    using System;

    public class Filters
    {
        private string filterName;
        private NavitaireTE.FilterType filterType;
        private string filterValue;

        public Filters(string filterName, string filterVale, NavitaireTE.FilterType filterType)
        {
            this.FilterName = filterName;
            this.FilterValue = filterVale;
            this.filterType = filterType;
        }

        public string FilterName
        {
            get
            {
                return this.filterName;
            }
            set
            {
                this.filterName = value;
            }
        }

        internal NavitaireTE.FilterType FilterType
        {
            get
            {
                return this.filterType;
            }
            set
            {
                this.filterType = value;
            }
        }

        public string FilterValue
        {
            get
            {
                return this.filterValue;
            }
            set
            {
                this.filterValue = value;
            }
        }
    }
}

