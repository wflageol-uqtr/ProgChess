import CookieProvider from "../providers/CookieProvider";
import ExerciseContent from "./ExerciseContent";

export default function Exercice() {
  return (
    <CookieProvider>
      <ExerciseContent />
    </CookieProvider>
  );
}
