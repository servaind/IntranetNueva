var PagerMaxPages = 5;
var PagerWindow = 2;

function DrawPager(page, totalPages) {
    var result = '';

    if (totalPages > 0) {
        result += '<ul class="pagination">';

        if (PagerShowFirstLast(totalPages)) result += '<li class="prev ' + (page <= 1 ? "disabled" : "") + '"><a href="javascript:' + (page <= 1 ? 'void(0)' : 'List(1)') + '"><i class="fa fa-angle-double-left"></i></a></li>';

        result += '<li class="prev ' + (page <= 1 ? "disabled" : "") + '"><a href="javascript:' + (page <= 1 ? 'void(0)' : ('List(' + (page - 1)) + ')') + '"><i class="fa fa-angle-left"></i></a></li>';

        var pageStart = PagerStartPage(page, totalPages);
        var pageEnd = PagerEndPage(page, totalPages);

        for (var i = pageStart; i <= pageEnd; i++) {
            result += '<li class="' + (i == page ? "active" : "") + '"><a href="javascript:' + (i == page ? 'void(0)' : 'List(' + i + ')') + '">' + i + '</a></li>';
        }

        result += '<li class="next ' + (page >= totalPages ? "disabled" : "") + '"><a href="javascript:' + (page >= totalPages ? 'void(0)' : ('List(' + (page + 1)) + ')') + '"><i class="fa fa-angle-right"></i></a></li>';

        if (PagerShowFirstLast(totalPages)) result += '<li class="prev ' + (page >= totalPages ? "disabled" : "") + '"><a href="javascript:' + (page >= totalPages ? 'void(0)' : 'List(' + totalPages + ')') + '"><i class="fa fa-angle-double-right"></i></a></li>';

        result += '</ul>';
    }

    return result;
}

function PagerShowFirstLast(totalPages) {
    return totalPages > PagerMaxPages;
}

function PagerStartPage(page, totalPages) {
    if (totalPages < PagerMaxPages) return 1;
    else {
        if (page == 1) return page;
        else {
            if (page - PagerWindow <= 0) return 1;
            else {
                if (page == totalPages) return page - 2 * PagerWindow;
                return page - PagerWindow;
            }
        }
    }
}

function PagerEndPage(page, totalPages) {
    if (totalPages < PagerMaxPages) return totalPages;
    else {
        if (page == totalPages) return page;
        else {
            if (page + PagerWindow >= totalPages) return totalPages;
            else {
                if (page == 1) return page + 2 * PagerWindow;
                else if (page == 2) return page + PagerWindow + 1;
                return page + PagerWindow;
            }
        }
    }
}