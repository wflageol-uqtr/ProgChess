import CookieProvider from "../providers/CookieProvider";
import ExerciceContent from "./ExerciceContent";

export default function Exercice() {
  return (
    <CookieProvider>
      <ExerciceContent />
    </CookieProvider>
  );
}
