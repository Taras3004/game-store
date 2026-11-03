// ReSharper disable All
import { check } from "k6";
import http from "k6/http";

export const options = {
    stages: [
        { duration: "10s", target: 20 },
        { duration: "50s", target: 20}
    ]
};

export default function() {
    const apiUrl = "https://localhost:7153";

    const request = {
        "game": {
            "name": "string",
            "key": "key1",
            "description": "string",
            "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        },
        "genres": [
            "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ],
        "platforms": [
            "3fa85f64-5717-4562-b3fc-2c963f66afa6"
        ]
    }

    const response = http.post(`${apiUrl}/api/Games`,
        JSON.stringify(request),
        {
            headers: { 'Content-Type': 'application/json' }
        });

    check(response,
        {
            'response code was 409': (res) => res.status == 409
        })
}