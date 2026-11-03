import { CommentType } from "./CommentType";

export interface GameComment {
    id: number;
    authorName: string;
    type: CommentType;
    date: string;
    text: string;
    rating?: number;
    replies?: GameComment[];
    quoteText?: string;
}
