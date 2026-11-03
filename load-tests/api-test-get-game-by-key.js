// ReSharper disable All
import { check } from "k6";
import http from "k6/http";

export const options = {
    stages: [
        { duration: "10s", target: 20 },
        { duration: "50s", target: 20 }
    ]
};

export default function () {
    const apiUrl = "https://localhost:7153";


    const response = http.get(`${apiUrl}/api/Games/some-key`);

    check(response, {
        "status is 200": (r) => r.status === 200,
    });
}