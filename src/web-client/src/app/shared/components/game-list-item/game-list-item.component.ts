import { Component, EventEmitter, Input, Output } from "@angular/core";
import { CurrencyPipe } from "@angular/common";
import { GameItem } from "../../../models/GameListItem";

@Component({
    selector: "game-list-item",
    standalone: true,
    imports: [CurrencyPipe],
    templateUrl: "game-list-item.component.html",
})
export class GameListItemComponent {
    @Input({ required: true }) game!: GameItem;
    @Output() selectGame = new EventEmitter<GameItem>();

    onClick(): void {
        this.selectGame.emit(this.game);
    }
}
