import {
  Sheet,
  SheetContent,
  SheetHeader,
  SheetTitle,
} from "../../components/ui/sheet";
import type { TestResult } from "../../utils/type";

interface ExecutionSheetProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  error?: string;
  testResult?: TestResult[];
}

export default function ExecutionSheet({
  open,
  onOpenChange,
  error,
  testResult,
}: ExecutionSheetProps) {
  return (
    <Sheet open={open} onOpenChange={onOpenChange}>
      <SheetContent className="bg-zinc-900 text-white overflow-auto">
        <SheetHeader>
          <SheetTitle className="text-white">
            Résultats de l'exécution
          </SheetTitle>
        </SheetHeader>
        <div className="space-y-4">
          {error ? (
            <div className="flex px-2 items-start gap-3">
              <div>
                <h3 className="text-red-300 font-semibold">Erreur détectée</h3>
                <p className="text-red-200 text-sm whitespace-pre-wrap mt-1">
                  {error}
                </p>
              </div>
            </div>
          ) : testResult?.length ? (
            testResult.map((test, index) => (
              <div
                key={index}
                className={`p-3 rounded-lg flex items-center justify-between ${
                  test.success ? "bg-green-600" : "bg-red-600"
                }`}
              >
                {/* Regarder type d'erreur Mardi avec william */}
                <span>{test.testName}</span>
                <span>{test.success ? "✅ Réussi" : "❌ Échoué"}</span>
              </div>
            ))
          ) : (
            <div>Aucun résultat</div>
          )}
        </div>
      </SheetContent>
    </Sheet>
  );
}
