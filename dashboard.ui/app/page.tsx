import { ReactNode } from "react";

export default function DashboardPage({ children }: { children: ReactNode }) {
  return (
    <div className="flex flex-col h-screen overflow-hidden">
      
      {/* --- FULL WIDTH HEADER --- */}
      <header className="navbar bg-base-100 border-b border-base-300 z-30 w-full shrink-0">
        <div className="flex-none lg:hidden">
          
          {/* Mobile Toggle Button */}
          <label htmlFor="main-drawer" className="btn btn-square btn-ghost">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" className="inline-block w-6 h-6 stroke-current">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M4 6h16M4 12h16M4 18h16"></path>
            </svg>
          </label>
        </div>
        <div className="flex-1 px-2 mx-2">
          <span className="text-lg font-bold">Dashboard.UI</span>
        </div>
        <div className="flex-none gap-2">
          <button className="btn btn-ghost btn-circle">
            <div className="indicator">
              <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9" /></svg>
              <span className="badge badge-xs badge-primary indicator-item"></span>
            </div>
          </button>
          <div className="avatar placeholder">
            <div className="bg-neutral text-neutral-content rounded-full w-8 flex items-center justify-center">
              <span>DT</span>
            </div>
          </div>
        </div>
      </header>

      {/* --- DRAWER AREA (Below Header) --- */}
      <div className="drawer lg:drawer-open flex-1 overflow-hidden">
        <input id="main-drawer" type="checkbox" className="drawer-toggle" />
        
        {/* --- MAIN DYNAMIC CONTENT AREA --- */}
        <main className="drawer-content flex flex-col overflow-y-auto bg-base-200">
          <div className="p-6">
            {children}
          </div>
        </main>

        {/* --- SIDEBAR --- */}
        <aside className="drawer-side z-20">
          <label htmlFor="main-drawer" aria-label="close sidebar" className="drawer-overlay"></label>
          <ul className="menu p-4 w-64 min-h-full bg-base-100 text-base-content border-r border-base-300">
            <li className="menu-title">Analytics</li>
            <li><a className="active">Heatmaps</a></li>
            <li><a>Trading Logs</a></li>
            <li className="menu-title mt-4">System</li>
            <li><a>Settings</a></li>
            <li><a>API Keys</a></li>
          </ul>
        </aside>
      </div>
    </div>
  );
}