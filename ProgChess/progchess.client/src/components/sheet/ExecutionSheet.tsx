import { AccordionItem } from "@radix-ui/react-accordion";
import {
  Sheet,
  SheetContent,
  SheetHeader,
  SheetTitle,
} from "../../components/ui/sheet";
import type { TestResult } from "../../utils/type";
import { Accordion, AccordionContent, AccordionTrigger } from "../ui/accordion";

interface ExecutionSheetProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  testResult?: TestResult[];
}

export default function ExecutionSheet({
  open,
  onOpenChange,
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
          {testResult?.length ? (
            <Accordion className="mx-2 space-y-3" type="single" collapsible>
              {testResult.map((test, index) => (
                <AccordionItem className="rounded-none" value={`item-${index}`}>
                  <AccordionTrigger
                    className={`flex cursor-pointer items-center justify-between px-4 py-3 text-white font-medium transition rounded-none ${
                      test.success
                        ? "bg-green-500 hover:bg-green-600"
                        : "bg-red-500 hover:bg-red-600"
                    }`}
                  >
                    {test.testName}
                  </AccordionTrigger>
                  <AccordionContent
                    className={`px-4 py-3 ${
                      test.success
                        ? "bg-green-100 text-green-500"
                        : "bg-red-100 text-red-500"
                    }`}
                  >
                    <p>{test.actual || "Aucun information disponible"}</p>
                    <p>{test.expected}</p>
                  </AccordionContent>
                </AccordionItem>
              ))}
            </Accordion>
          ) : (
            <div className="flex justify-center">Aucun résultat</div>
          )}
        </div>
      </SheetContent>
    </Sheet>
  );
}
