import { Component, inject } from "@angular/core";
import { GameFilterComponent } from "../../shared/components/game-filter/game-filter.component";
import { FilterOption } from "../../models/FilterOption";
import { GameItem } from "../../models/GameListItem";
import { GameListComponent } from "../../shared/components/game-list/game-list.component";

@Component({
    selector: "game-list-page",
    standalone: true,
    imports: [GameListComponent, GameFilterComponent],
    templateUrl: "./game-list-page.component.html",
})
export class GameListPageComponent {
    genres: FilterOption[] = [
        { id: "action", name: "Action" },
        { id: "adventure", name: "Adventure" },
        { id: "arcade", name: "Arcade" },
        { id: "casual", name: "Casual" },
        { id: "educational", name: "Educational" },
        { id: "family", name: "Family" },
        { id: "fighting", name: "Fighting" },
        { id: "music", name: "Music/Rhythm" },
        { id: "party", name: "Party" },
        { id: "puzzle", name: "Puzzle" },
        { id: "racing", name: "Racing" },
        { id: "rpg", name: "Role-Playing Games" },
        { id: "shooter", name: "Shooter" },
        { id: "simulation", name: "Simulation" },
        { id: "sports", name: "Sports" },
        { id: "strategy", name: "Strategy" },
    ];

    platforms: FilterOption[] = [
        { id: "ps4", name: "PS4" },
        { id: "ps3", name: "PS3" },
        { id: "psvita", name: "PS Vita" },
    ];

    publishers: FilterOption[] = [
        { id: "ea", name: "EA Games" },
        { id: "blizzard", name: "Blizzard" },
        { id: "valve", name: "Valve" },
        { id: "rockstar", name: "Rockstar Games" },
        { id: "ubisoft", name: "Ubisoft" },
        { id: "cd_projekt", name: "CD Projekt" },
    ];

    games: GameItem[] = [
        { id: "1", title: "Genesis Alpha One", platform: "PS4", price: 29.99 },
        { id: "2", title: "Kingdom Hearts HD 1.5 + 2.5 ReMIX", platform: "PS4", price: 49.99 },
        { id: "3", title: "Kingdom Hearts III", platform: "PS4", price: 59.99 },
        { id: "4", title: "Resident Evil 2", platform: "PS4", price: 59.99 },
        { id: "5", title: "Resident Evil 2 Deluxe Edition", platform: "PS4", price: 69.99 },
        {
            id: "6",
            title: "Ace Combat 7: Skies Unknown Launch Edition",
            platform: "PS4",
            price: 59.99,
            discountedPrice: 49.99,
        },
        { id: "7", title: "Eden Tomorrow", platform: "PS4 / PS VR", price: 39.99 },
        {
            id: "8",
            title: "Madden NFL 19: Ultimate Super Bowl Edition",
            platform: "PS4",
            price: 79.99,
        },
        { id: "9", title: "Paladins Champion Pack", platform: "PS4", price: 29.99 },
        { id: "10", title: "The Grand Tour Game", platform: "PS4", price: 14.99 },
        { id: "11", title: "Vane", platform: "PS4", price: 24.99 },
        { id: "12", title: "Hitman HD Enhanced Collection", platform: "PS4", price: 59.99 },
    ];

    onGameSelect(game: GameItem): void {
        console.log("Navigating to game details for:", game.title);
    }
}
