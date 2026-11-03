import { Component, EventEmitter, Input, Output } from "@angular/core";
import { CurrencyPipe } from "@angular/common";
import { GameDetails } from "../../../models/GameDetails";

@Component({
    selector: "game-details",
    standalone: true,
    imports: [CurrencyPipe],
    templateUrl: "./game-details.component.html",
})
export class GameDetailsComponent {
    @Input({ required: true }) game!: GameDetails;
    @Output() addToCart = new EventEmitter<GameDetails>();

    onAddToCart(): void {
        this.addToCart.emit(this.game);
    }
}
