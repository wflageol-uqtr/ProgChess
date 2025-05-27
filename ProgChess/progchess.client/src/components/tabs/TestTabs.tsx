import { useState } from "react";
import type { UnitTest } from "../../utils/type";

interface TabsPros {
  tabs: UnitTest[];
}

export default function TestTabs({ tabs }: TabsPros) {
  const [activeTab, setActiveTab] = useState(0);

  return (
    <div className="px-2">
      {tabs && tabs.length && (
        <>
          <div className="flex flex-wrap border-b-gray-400 py-2 gap-x-2">
            {tabs.map((tab, index) => (
              <button
                className={`py-2 px-6 font-medium text-sm rounded-lg cursor-pointer hover:bg-zinc-600 ${
                  activeTab === index ? "bg-zinc-600" : ""
                }`}
                onClick={() => setActiveTab(index)}
                key={index}
              >{`Test ${index + 1}`}</button>
            ))}
          </div>
          <div className="flex flex-col lg:flex-row gap-6">
            <div className="w-full lg:w-2/3 space-y-4">
              <div className="flex items-start gap-2 bg-red-500/10 border border-red-400 text-red-700 px-3 py-2 rounded-md">
                <svg
                  className="w-4 h-4 mt-0.5 flex-shrink-0 text-red-500"
                  fill="none"
                  stroke="currentColor"
                  strokeWidth="2"
                  viewBox="0 0 24 24"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    d="M12 9v2m0 4h.01M4.93 4.93l14.14 14.14M4.93 19.07L19.07 4.93"
                  />
                </svg>
                <p className="text-sm">
                  La sortie ne correspond pas au résultat attendu.{" "}
                  <span className="font-medium">
                    Merci de vérifier votre code.
                  </span>
                </p>
              </div>

              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div className="space-y-1.5">
                  <label className="text-sm font-medium text-zinc-200">
                    Sortie du test
                  </label>
                  <div className="bg-zinc-700 text-zinc-100 py-2 px-3 rounded-md text-sm">
                    {tabs[activeTab].code}
                  </div>
                </div>
                <div className="space-y-1.5">
                  <label
                    htmlFor="expected"
                    className="text-sm font-medium text-zinc-200"
                  >
                    Votre sortie
                  </label>
                  <div className="bg-zinc-700 text-zinc-100 py-2 px-3 rounded-md text-sm">
                    Rouler les tests...
                  </div>
                </div>
              </div>
            </div>

            <div className="w-full lg:w-1/3">
              <label
                htmlFor="terminal"
                className="block mb-2 text-sm font-medium text-zinc-200"
              >
                Terminal
              </label>
              <textarea
                id="terminal"
                name="terminal"
                disabled
                className="bg-zinc-700 text-zinc-100 w-full h-full min-h-[100px] rounded-lg px-3 py-2 text-sm resize-none"
                placeholder="Sortie du terminal..."
              ></textarea>
            </div>
          </div>
        </>
      )}
    </div>
  );
}
