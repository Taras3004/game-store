import { Component, EventEmitter, Input, Output } from "@angular/core";
import { FilterOption } from "../../../models/FilterOption";

@Component({
    selector: "game-filter",
    standalone: true,
    templateUrl: "./game-filter.component.html",
})
export class GameFilterComponent {
    @Input() genres: FilterOption[] = [];
    @Input() platforms: FilterOption[] = [];
    @Input() publishers: FilterOption[] = [];

    releaseDates: FilterOption[] = [
        { id: "coming_soon", name: "Coming Soon" },
        { id: "last_30_days", name: "Last 30 Days" },
        { id: "last_90_days", name: "Last 90 Days" },
    ];

    sortOptions: FilterOption[] = [
        { id: "best_match", name: "Best Match" },
        { id: "price_asc", name: "Price: Low to High" },
        { id: "price_desc", name: "Price: High to Low" },
    ];
}
