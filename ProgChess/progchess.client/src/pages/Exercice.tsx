import { useEffect } from "react";
import { useNavigate } from "react-router";
import { hasCookie } from "../utils/cookie";

export default function Exercice() {
  const navigate = useNavigate();

  useEffect(() => {
    if (!hasCookie("studentCookie")) {
      navigate("/login");
    }
  });

  return <div>exercice 1</div>;
}
